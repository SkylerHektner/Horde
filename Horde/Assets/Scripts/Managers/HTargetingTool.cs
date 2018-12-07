using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HTargetingTool : MonoBehaviour {

    public static HTargetingTool Instance;

    public delegate void unitReadyCallback(Unit selectedUnit, bool success);

    public delegate void positionReadyCallback(Vector3 positon, bool success);

    public delegate void intReadyCallback(int value, bool success);

    public delegate void unitOrPlayerReadyCallback(object unitOrPlayer, bool player, bool success);

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
    private GameObject radialMenuUI;
    [SerializeField]
    private GameObject intInputScrim;
    [SerializeField]
    private IncrementableText intInputText;
    [SerializeField]
    private GameObject selectionRadiusIndicator;
    [SerializeField]
    private float selectionRadius = 10f;

    private bool intInputReady = false;
    private bool abortCurrentRequest = false;

    private int layerMask = 1 << 15; // Layer mask for the background.

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
        selectionRadiusIndicator.transform.localScale *= selectionRadius;
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
            if (currentRequest.unitCallback != null || currentRequest.unitOrPlayerCallback != null)
            {
                selectionRadiusIndicator.SetActive(true);
                selectionRadiusIndicator.transform.position = currentRequest.callingUnit.transform.position + new Vector3(0, -1f, 0);
            }
        }

        else if (Executing)
        {
            // CHECK IF THE PLAYER REQUESTED TO ABORT THE CURRENT REQUEST
            if (abortCurrentRequest)
            {
                ReturnToNormal();
                if (currentRequest.posCallback != null)
                {
                    currentRequest.posCallback(Vector3.zero, false);
                }
                else if (currentRequest.intCallback != null)
                {
                    currentRequest.intCallback(0, false);
                }
                else if (currentRequest.unitCallback != null)
                {
                    currentRequest.unitCallback(null, false);
                }
                else if (currentRequest.unitOrPlayerCallback != null)
                {
                    currentRequest.unitOrPlayerCallback(null, false, false);
                }
                abortCurrentRequest = false;
                Executing = false;
            }

            // CHECK IF POSITION READY
            if (currentRequest.posCallback != null)
            {
                Vector3 pos = tryGetPos();
                if (pos != Vector3.zero)
                {
                    ReturnToNormal();
                    currentRequest.posCallback(pos, true);
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
                    currentRequest.intCallback(value, true);
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
                    currentRequest.unitCallback(u, true);
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
                        currentRequest.unitOrPlayerCallback(u, true, true);
                    }
                    else
                    {
                        currentRequest.unitOrPlayerCallback(u, false, true);
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
        Vector3 pos = new Vector3(currentRequest.callingUnit.transform.position.x, currentRequest.callingUnit.transform.position.y, currentRequest.callingUnit.transform.position.z);
        cam.SetTargetPos(pos);
        cam.lockPanControls = false;
        rimCanvas.SetActive(true);
        instructionText.text = message;
        radialMenuUI.SetActive(false);

        OnTargeting();
    }

    /// 6. remove rim and instruction text, disable camera WASD and resume time
    private void ReturnToNormal()
    {
        intInputScrim.SetActive(false);
        selectionRadiusIndicator.SetActive(false);
        cam.lockPanControls = true;
        rimCanvas.SetActive(false);
        radialMenuUI.SetActive(true);

        OnFinishedTargeting();
    }

    private Unit tryGetUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.MaxValue, ~layerMask))
            {
                Unit u = hitInfo.collider.gameObject.GetComponent<Unit>();
                if (u != null && (u.transform.position - currentRequest.callingUnit.transform.position).magnitude <= selectionRadius)
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.MaxValue, ~layerMask))
            {
                Unit u = hitInfo.collider.gameObject.GetComponent<Unit>();
                if (u != null && (u.transform.position - currentRequest.callingUnit.transform.position).magnitude <= selectionRadius)
                {
                    return u;
                }
                PlayerMovement pm = hitInfo.collider.gameObject.GetComponent<PlayerMovement>();
                if (pm != null && (pm.transform.position - currentRequest.callingUnit.transform.position).magnitude <= selectionRadius)
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.MaxValue, ~layerMask))
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

    public void SetAbortCurRequestFlag()
    {
        abortCurrentRequest = true;
    }
}
