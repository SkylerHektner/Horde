﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HTargetingTool : MonoBehaviour {

    public static HTargetingTool Instance;

    public delegate void unitReadyCallback(Unit selectedUnit);

    public delegate void positionReadyCallback(Vector3 positon);

    public delegate void intReadyCallback(int value);

    public delegate void unitOrPlayerReadyCallback(object unitOrPlayer, bool player);

    public delegate void TargetAction();
    public static event TargetAction OnTargeting;

    public delegate void FinishedTargetAction();
    public static event FinishedTargetAction OnFinishedTargeting;

    [SerializeField]
    private CameraController cam;
    [SerializeField]
    private PlayerMovement pMovement;
    [SerializeField]
    private GameObject rimCanvas;
    [SerializeField]
    private Text instructionText;
    [SerializeField]
    private GameObject classEditor;
    [SerializeField]
    private GameObject intInputScrim;
    [SerializeField]
    private IncrementableText intInputText;

    private bool intInputReady = false;

    /// <summary>
    /// 2. Move the camera to the calling unit, Unlock camera WASD movement
    /// 3. Pause everything (stop all units from executing heurstics and moving. Also stop spell projectiles currently travelling)
    /// 4. Add rim effect + instructions text(a new canvas)
    /// 5. Check every frame for a selected position or target (mouse down raycast into acceptable target)
    /// 6. remove rim and instruction text disable camera WASD and resume time
    /// </summary>
    /// 

    public bool GettingInput { get { return Executing; } }

    private bool Executing = false;
    private Queue<pendingRequest> pendingRequests = new Queue<pendingRequest>();
    private pendingRequest currentRequest;
    struct pendingRequest
    {
        public Unit callingUnit;
        public unitReadyCallback unitCallback;
        public positionReadyCallback posCallback;
        public intReadyCallback intCallback;
        public unitOrPlayerReadyCallback unitOrPlayerCallback;
        public string message;
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (pendingRequests.Count > 0 && !Executing)
        {
            currentRequest = pendingRequests.Dequeue();
            Executing = true;
            Setup(currentRequest.message);
            if (currentRequest.intCallback != null)
            {
                intInputScrim.SetActive(true);
            }
        }

        else if (Executing)
        {
            // CHECK IF POSITION READY
            if (currentRequest.posCallback != null)
            {
                Vector3 pos = tryGetPos();
                if (pos != Vector3.zero)
                {
                    ReturnToNormal();
                    currentRequest.posCallback(pos);
                    Executing = false;
                }
            }
            // CHECK IF INT READY
            else if (currentRequest.intCallback != null)
            {
                int value = tryGetInt();
                if (value != -1)
                {
                    ReturnToNormal();
                    currentRequest.intCallback(value);
                    Executing = false;
                }
            }
            // CHECK IF UNIT READY
            else if (currentRequest.unitCallback != null)
            {
                Unit u = tryGetUnit();
                if (u != null)
                {
                    ReturnToNormal();
                    currentRequest.unitCallback(u);
                    Executing = false;
                }
            }
            // CHECK IF UNIT OR PLAYER READY
            else if (currentRequest.unitOrPlayerCallback != null)
            {
                object u = tryGetUnitOrPlayer();
                if (u != null)
                {
                    ReturnToNormal();
                    if(u.GetType() == typeof(PlayerMovement))
                    {
                        currentRequest.unitOrPlayerCallback(u, true);
                    }
                    else
                    {
                        currentRequest.unitOrPlayerCallback(u, false);
                    }
                    Executing = false;
                }
            }
        }
    }

	public void GetTarget(Unit callingUnit, unitReadyCallback unitReadyCallbackMethod, string message)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.unitCallback = unitReadyCallbackMethod;
        r.message = message;
        pendingRequests.Enqueue(r);
    }

    public void GetPositon(Unit callingUnit, positionReadyCallback positionReadyCallbackMethod, string message)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.posCallback = positionReadyCallbackMethod;
        r.message = message;
        pendingRequests.Enqueue(r);
    }

    public void GetInt(Unit callingUnit, intReadyCallback intReadyCallbackMethod, string message)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.intCallback = intReadyCallbackMethod;
        r.message = message;
        pendingRequests.Enqueue(r);
    }

    public void GetUnitOrPlayer(Unit callingUnit, unitOrPlayerReadyCallback unitOrPlayerReadyCallbackMethod, string message)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.unitOrPlayerCallback = unitOrPlayerReadyCallbackMethod;
        r.message = message;
        pendingRequests.Enqueue(r);
    }

    /// 2. Move the camera to the calling unit, Unlock camera WASD movement
    /// 3. Pause everything (stop all units from executing heurstics and moving. Also stop spell projectiles currently travelling)
    /// 4. Add rim effect + instructions text(a new canvas)
    private void Setup(string message)
    {
        cam.SetTargetPos(currentRequest.callingUnit.transform.position.x, currentRequest.callingUnit.transform.position.z);
        cam.lockWASDPanControls = false;
        rimCanvas.SetActive(true);
        instructionText.text = message;
        classEditor.SetActive(false);

        OnTargeting();
    }

    /// 6. remove rim and instruction text, disable camera WASD and resume time
    private void ReturnToNormal()
    {
        intInputScrim.SetActive(false);
        cam.lockWASDPanControls = true;
        rimCanvas.SetActive(false);
        classEditor.SetActive(true);

        OnFinishedTargeting();
    }

    private Unit tryGetUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                Unit u = hitInfo.collider.gameObject.GetComponent<Unit>();
                if (u != null)
                {
                    return u;
                }
            }
        }
        return null;
    }

    private object tryGetUnitOrPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                Unit u = hitInfo.collider.gameObject.GetComponent<Unit>();
                if (u != null)
                {
                    return u;
                }
                PlayerMovement pm = hitInfo.collider.gameObject.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    return pm;
                }
            }
        }
        return null;
    }

    private Vector3 tryGetPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                NavMeshPath p = new NavMeshPath(); ;
                if (currentRequest.callingUnit.GetComponent<NavMeshAgent>().CalculatePath(hitInfo.point, p))
                {
                    return hitInfo.point;
                }
            }
        }
        return Vector3.zero;
    }

    private int tryGetInt()
    {
        if(intInputReady)
        {
            intInputReady = false;
            return intInputText.Value;
        }
        return -1;
    }

    public void SetIntReadyFlag()
    {
        intInputReady = true;
    }
}
