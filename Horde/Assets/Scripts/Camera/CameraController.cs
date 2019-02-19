using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float transitionSpeed = 3.0f;
    private Transform targetTransform;
    
    private void Awake()
    {
        targetTransform = transform;
    }

    private void Update()
    {
        if(transform.position != targetTransform.position)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, transitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, transitionSpeed * Time.deltaTime);
        }
    }

    public void MoveTo(Transform t)
    {
        targetTransform = t;
    }
}
