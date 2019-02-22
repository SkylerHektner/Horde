using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTrigger : MonoBehaviour
{
    [SerializeField] private Transform newTransform;

    private CameraController cc;

    void Start()
    {
        cc = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
            cc.MoveTo(newTransform);
    }
}
