using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour, ITriggerHandler
{
    [SerializeField]
    [Header("Bridge Settings")]
    [Tooltip("The angle in degrees that the bridge will raise by")]
    public float OpenAngle;
    [Tooltip("The speed at which the bridge lifts")]
    public float smooth = 2.0f;
    [Tooltip("Check if you want the bridge to start up")]
    public bool StartUp;

    [Header("FOR TESTING ONLY (uncheck for actual build)")]
    [Tooltip("activating this makes the space bar toggle the door")]
    public bool SpaceBarTesting;

    private Quaternion target;
    private Quaternion original;
    private bool opening = false;

    public void Start()
    {
        original = transform.localRotation;
        if(StartUp)
        {
            opening = true;
            transform.rotation = Quaternion.Euler(original.eulerAngles.x - OpenAngle, original.eulerAngles.y, original.eulerAngles.z);
        }
       // CloseAngle = transform.rotation.eulerAngles.y;
    }
    public void TriggerOff()
    {
        opening = false;
    }

    public void TriggerOn()
    {
        opening = true;
    }


    /// <summary>
    /// code to rotate a door around its axis if opening or closing based on
    /// Ingen's reply to "how to make a solid door open and close?
    /// found at: https://answers.unity.com/questions/294194/how-to-make-a-solid-door-open-and-close.html
    /// 
    /// and the hinging method found on "[Unity 3d tutorial] Triggering a door animation from mouse click select"
    /// by hdsenevi found at: https://www.youtube.com/watch?v=scm7r0uBepU&noredirect=1
    /// </summary>
    public void Update()
    {
        // set the target to the open position
        if (opening)
        {
            target = Quaternion.Euler(original.eulerAngles.x - OpenAngle, original.eulerAngles.y, original.eulerAngles.z);
        }
        // set the target to teh close position
        else
        {
            target = original;// Quaternion.Euler(0, CloseAngle, 0);
        }

        //smoothly transform the doors position to the new rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);

        //for easy testing of the door
        if (SpaceBarTesting && Input.GetKeyDown("space"))
        {
            opening = !opening;
        }
    }
}
