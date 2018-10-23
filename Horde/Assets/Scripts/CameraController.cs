using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private ClassEditorUI classEditorUI;
    [SerializeField]
    private float camMoveSpeed = 5f;
    [SerializeField]
    private float camZoomSensitivity = 1f;
    [SerializeField]
    private float camZoomMax = 30f;
    [SerializeField]
    private float camZoomMin = 6f;

    public bool lockZoomControls = false;
    public bool lockPanControls = false;
    public bool lockWASDPanControls = false;
    public bool lockMousePanControls = true;
    public bool lockRotationControls = false;

    private Vector3 targetRot = Vector3.zero;
    private Vector3 targetPos;
    private float targetZoom;

    private void Start()
    {
        targetPos = transform.position;
        targetZoom = cam.fieldOfView;
        targetRot = transform.rotation.eulerAngles;
        Instance = this;
    }

    private void Update()
    {
        // If the user is editing classes just return
        if (classEditorUI != null && classEditorUI.InEditMode)
        {
            return;
        }

        if(!lockPanControls)
        {
            if ((!lockMousePanControls && Input.mousePosition.x < 0)
            || (!lockWASDPanControls && Input.GetKey(KeyCode.A)))
            {
                targetPos += -transform.right * camMoveSpeed * Time.deltaTime;
            }
            else if ((!lockMousePanControls && Input.mousePosition.x > Screen.width)
                || (!lockWASDPanControls && Input.GetKey(KeyCode.D)))
            {
                targetPos += transform.right * camMoveSpeed * Time.deltaTime;
            }
            if ((!lockMousePanControls && Input.mousePosition.y < 0)
                || (!lockWASDPanControls && Input.GetKey(KeyCode.S)))
            {
                Vector3 delta = -transform.forward;
                delta.y = 0;
                targetPos += delta * camMoveSpeed * Time.deltaTime;
            }
            else if ((!lockMousePanControls && Input.mousePosition.y > Screen.height)
                || (!lockWASDPanControls && Input.GetKey(KeyCode.W)))
            {
                Vector3 delta = transform.forward;
                delta.y = 0;
                targetPos += delta * camMoveSpeed * Time.deltaTime; ;
            }
        }
        

        // Camera Zoom Controls
        if (!lockZoomControls)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (targetZoom < camZoomMax)
                {
                    targetZoom += camZoomSensitivity;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (targetZoom > camZoomMin)
                {
                    targetZoom -= camZoomSensitivity;
                }
            }
        }
        
        if (!lockRotationControls)
        {
            // Camera Rotate Controls
            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRot.y += 90;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                targetRot.y -= 90;
            }
        }

        //LERPING TO MAKE TRANSITIONS SMOOTH
        if (transform.rotation.eulerAngles != targetRot)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime * 5f);
        }
        if(targetPos != transform.position)
        {
            transform.position = Vector3.Lerp(
                transform.position, targetPos, Time.deltaTime * 5f);
        }
        if (targetZoom != cam.orthographicSize)
        {
            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize, targetZoom, Time.deltaTime * 5f);
            cam.fieldOfView = cam.orthographicSize;
        }
    }

    /// <summary>
    /// Use me to set the target position of the camera! I'll be staring right at the x,z you give me
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetTargetPos(float x, float z)
    {
        targetPos.x = x;
        targetPos.z = z;
    }

    /// <summary>
    /// Use me to set the target zoom level (3 = crazy zoom, 30 = very zoomed out)
    /// </summary>
    /// <param name="zoom"></param>
    public void setTargetZoom(int zoom)
    {
        if (zoom < camZoomMin)
        {
            targetZoom = camZoomMin;
        }
        else if (zoom > camZoomMax)
        {
            targetZoom = camZoomMax;
        }
        else
        {
            targetZoom = zoom;
        }
    }
}
