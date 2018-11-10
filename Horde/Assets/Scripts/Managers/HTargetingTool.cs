using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HTargetingTool : MonoBehaviour {

    public static HTargetingTool Instance;

    public delegate void unitReadyCallback(Unit selectedUnit);

    public delegate void positionReadyCallback(Vector3 positon);

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

    /// <summary>
    /// 1. Tell player to unlock the camera, stop player WASD movement
    /// 2. Move the camera to the calling unit, Unlock camera WASD movement
    /// 3. Pause everything (stop all units from executing heurstics and moving. Also stop spell projectiles currently travelling)
    /// 4. Add rim effect + instructions text(a new canvas)
    /// 5. Check every frame for a selected position or target (mouse down raycast into acceptable target)
    /// 6. Return camera to player, remove rim and instruction text, enable player WASD and disable camera WASD and resume time
    /// </summary>
    /// 
    private bool Executing = false;
    private Queue<pendingRequest> pendingRequests = new Queue<pendingRequest>();
    private pendingRequest currentRequest;
    struct pendingRequest
    {
        public Unit callingUnit;
        public unitReadyCallback unitCallback;
        public positionReadyCallback posCallback;
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
            Setup();
        }
        else if (Executing)
        {
            // if the current request is looking for a position
            if (currentRequest.unitCallback == null)
            {
                Vector3 pos = tryGetPos();
                if (pos != Vector3.zero)
                {
                    ReturnToNormal();
                    currentRequest.posCallback(pos);
                    Executing = false;
                }
            }
            else
            {
                Unit u = tryGetUnit();
                if (u != null)
                {
                    ReturnToNormal();
                    currentRequest.unitCallback(u);
                    Executing = false;
                }
            }
        }
    }

	public void GetTarget(Unit callingUnit, unitReadyCallback unitReadyCallbackMethod)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.unitCallback = unitReadyCallbackMethod;
        pendingRequests.Enqueue(r);
    }

    public void GetPositon(Unit callingUnit, positionReadyCallback positionReadyCallbackMethod)
    {
        pendingRequest r = new pendingRequest();
        r.callingUnit = callingUnit;
        r.posCallback = positionReadyCallbackMethod;
        pendingRequests.Enqueue(r);
    }

    /// 1. Tell player to unlock the camera, stop player WASD movement
    /// 2. Move the camera to the calling unit, Unlock camera WASD movement
    /// 3. Pause everything (stop all units from executing heurstics and moving. Also stop spell projectiles currently travelling)
    /// 4. Add rim effect + instructions text(a new canvas)
    private void Setup()
    {
        pMovement.lockCamToPlayer = false;
        pMovement.lockWASDControls = true;
        cam.SetTargetPos(currentRequest.callingUnit.transform.position.x, currentRequest.callingUnit.transform.position.z);
        cam.lockWASDPanControls = false;
        rimCanvas.SetActive(true);
        if (currentRequest.unitCallback == null)
        {
            instructionText.text = "Please click where you want this unit to move";
        }
        else
        {
            instructionText.text = "Please click the unit you want this unit to target";
        }
        classEditor.SetActive(false);
        //TODO: This method still needs to pause time
    }

    /// 6. Return camera to player, remove rim and instruction text, enable player WASD and disable camera WASD and resume time
    private void ReturnToNormal()
    {
        pMovement.lockCamToPlayer = true;
        pMovement.lockWASDControls = false;
        cam.lockWASDPanControls = true;
        rimCanvas.SetActive(false);
        classEditor.SetActive(true);
        //TODO: This method still needs to resume time
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
}
