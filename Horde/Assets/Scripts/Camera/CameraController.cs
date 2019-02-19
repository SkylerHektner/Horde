using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private int numTransitionFrames = 100;
    private Transform targetTransform;

    private int curTransitionFrame = 20;
    
    private void Awake()
    {
        targetTransform = transform;
    }

    private void FixedUpdate()
    {
        if(curTransitionFrame != numTransitionFrames)
        {
            curTransitionFrame++;
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, (float)curTransitionFrame / numTransitionFrames);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, (float)curTransitionFrame / numTransitionFrames);
        }
    }

    public void MoveTo(Transform t)
    {
        targetTransform = t;
        curTransitionFrame = 0;
    }
}
