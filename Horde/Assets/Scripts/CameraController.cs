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
            transform.position += -transform.right * camMoveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x > Screen.width || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * camMoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < 0 || Input.GetKey(KeyCode.S))
        {
            Vector3 delta = -transform.forward;
            delta.y = 0;
            transform.position += delta * camMoveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y > Screen.height || Input.GetKey(KeyCode.W))
        {
            Vector3 delta = transform.forward;
            delta.y = 0;
            transform.position += delta * camMoveSpeed * Time.deltaTime; ;
        }

        // Camera Zoom Controls
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cam.orthographicSize < camZoomMax)
            {
                cam.orthographicSize += camZoomSensitivity;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cam.orthographicSize > camZoomMin)
            {
                cam.orthographicSize -= camZoomSensitivity;
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
        if (transform.rotation.eulerAngles != targetRot)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime * 5f);
        }
    }
}
