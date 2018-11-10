using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTargetingTool : MonoBehaviour {

    public static HTargetingTool Instance;

    public delegate void unitReadyCallback(Unit selectedUnit);

    public delegate void positionReadyCallback(Vector3 positon);

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
        if (!Executing && pendingRequests.Count > 0)
        {
            currentRequest = pendingRequests.Dequeue();
            Executing = true;
            Setup();
        }
        if (Executing)
        {

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

    private void Setup()
    {
        
    }

    private void ReturnToNormal()
    {

    }
}
