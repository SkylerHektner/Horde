using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour, ITriggerHandler
{

    public float OpenHeignt;

    public float OpenSpeed;

    public bool SpaceBarTesting;

    private Vector3 originalPosition;
    private Vector3 target;
    private bool opening;

    public void TriggerOff()
    {
        opening = false;
    }

    public void TriggerOn()
    {
        opening = true;
    }

    // Use this for initialization
    void Start () {
        originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if (opening)
        {
            target = originalPosition;
            target.y += OpenHeignt;
        }
        else
        {
            target = originalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, OpenSpeed * Time.deltaTime);

        if (SpaceBarTesting && Input.GetKeyDown("space"))
        {
            opening = !opening;
        }
	}
}
