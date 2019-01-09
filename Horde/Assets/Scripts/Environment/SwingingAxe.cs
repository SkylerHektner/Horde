using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour, ITriggerHandler {

    [Header("Axe Controls")]
    [Tooltip("Check to start swinging at launch")]
    public bool Active;
    [Tooltip("The Arc in Degrees of the swing from left end to right end")]
    public float Arc;
    [Tooltip("The speed the axe swings (Don't go more than 3)")]
    public float SwingSpeed;
    [Tooltip("A time reduction for return swings")]
    public float ReturnSpeed;
    [Tooltip("The Starting position of the axe")]
    public StartEnum StartPos;

    [Header("FOR TESTING ONLY (uncheck for actual build)")]
    [Tooltip("activating this makes the space bar toggle the swinging")]
    public bool SpaceBarTesting;

    private Quaternion leftAngle;
    private Quaternion rightAngle;
    private Quaternion target;
    private bool swingLeft = true;

    public enum StartEnum
    {
        Left,
        Right,
        Center
    }

    // Use this for initialization
    void Start()
    {
        leftAngle = Quaternion.Euler(transform.localEulerAngles.x + Arc/2, transform.localEulerAngles.y,
            transform.localEulerAngles.z);
        rightAngle = Quaternion.Euler(transform.localEulerAngles.x - Arc / 2, transform.localEulerAngles.y,
            transform.localEulerAngles.z);
    }


    public void TriggerOff()
    {
        Active = false;
    }

    public void TriggerOn()
    {
        Active = true;
    }

	// Update is called once per frame
	void Update () {
	    if(Active)
        {
            if(swingLeft)
            {
                target = leftAngle;
            }
            else
            {
                target = rightAngle;
            }
        }

        //.Log(Quaternion.Angle(transform.localRotation, target));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * SwingSpeed);


        if (Quaternion.Angle(transform.localRotation, target) < ReturnSpeed)
        {
            //Debug.Log("1");
            swingLeft = !swingLeft;
        }

        if(SpaceBarTesting && Input.GetKeyDown("space"))
        {
            Debug.Log("Space");
            Active = !Active;
        }
    }
}
