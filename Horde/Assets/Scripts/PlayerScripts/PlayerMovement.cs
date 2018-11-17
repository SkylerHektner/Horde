using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private CameraController cam;

    public bool lockCamToPlayer = true;
    public bool lockWASDControls = false;
    private bool inTargetingAction = false;

    private bool beingCarried = false;
    [SerializeField]
    private YesNoDialogue yesNoDioPrefab;
    private YesNoDialogue curDio;

    private Vector3 forward;
    private Vector3 right;

    private NavMeshAgent agent;
    private Animator anim;

    private Vector3 lastPos;
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        forward = new Vector3(0, 0, speed);
        right = new Vector3(speed, 0, 0);
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        HTargetingTool.OnTargeting += OnTargetingAction;
        HTargetingTool.OnFinishedTargeting += OnFinishedTargeting;
    }
	
	void Update ()
    {
        if (ClassEditorUI.Instance.InEditMode)
        {
            return;
        }

        bool walking = false;
        if (!lockWASDControls && !beingCarried)
        {
            if (Input.GetKey(KeyCode.W))
            {
                agent.Move(forward * Time.deltaTime);
                walking = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                agent.Move(forward * -Time.deltaTime);
                walking = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                agent.Move(right * -Time.deltaTime);
                walking = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                agent.Move(right * Time.deltaTime);
                walking = true;
            }
        }
        else if (beingCarried && !inTargetingAction)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (curDio == null)
                {
                    curDio = Instantiate(yesNoDioPrefab);
                    curDio.Init(untoggleCarryMode, null, "Would you like to stop being carried?");
                }
            }
        }
        anim.SetBool("Walking", walking);

        if (lastPos != transform.position && !beingCarried)
        {
            transform.forward = transform.position - lastPos;
            lastPos = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 temp = forward;
            forward = right;
            right = -temp;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 temp = forward;
            forward = -right;
            right = temp;
        }

        if (lockCamToPlayer)
        {
            cam.SetTargetPos(transform.position.x, transform.position.z);
        }
    }

    /// <summary>
    /// call this to set the player in carry mode.
    /// Disabled movement and makes a prompt if they try to move
    /// </summary>
    public void toggleCarryMode()
    {
        lockWASDControls = true;
        beingCarried = true;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    private void untoggleCarryMode()
    {
        transform.parent = null;
        lockWASDControls = false;
        beingCarried = false;
        GetComponent<NavMeshAgent>().enabled = true;
    }

    private void OnTargetingAction()
    {
        lockCamToPlayer = false;
        lockWASDControls = true;
        inTargetingAction = true;
    }

    private void OnFinishedTargeting()
    {
        lockCamToPlayer = true;
        lockWASDControls = false;
        inTargetingAction = false;
    }
}
