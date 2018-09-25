using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

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

    private Vector3 targetRot = Vector3.zero;
    private Vector3 targetPos;
    private float targetZoom;

    private void Start()
    {
        targetPos = transform.position;
        targetZoom = cam.orthographicSize;
        targetRot = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        // If the user is editing classes just return
        if (classEditorUI.InEditMode)
        {
            return;
        }

        // Camera Pan Controls
        if (Input.mousePosition.x < 0 || Input.GetKey(KeyCode.A))
        {
            targetPos += -transform.right * camMoveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x > Screen.width || Input.GetKey(KeyCode.D))
        {
            targetPos += transform.right * camMoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < 0 || Input.GetKey(KeyCode.S))
        {
            Vector3 delta = -transform.forward;
            delta.y = 0;
            targetPos += delta * camMoveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y > Screen.height || Input.GetKey(KeyCode.W))
        {
            Vector3 delta = transform.forward;
            delta.y = 0;
            targetPos += delta * camMoveSpeed * Time.deltaTime; ;
        }

        // Camera Zoom Controls
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

        // Camera Rotate Controls
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetRot.y += 90;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetRot.y -= 90;
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
}
