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
    }
	
	void Update ()
    {
        if (ClassEditorUI.Instance.InEditMode)
        {
            return;
        }

        bool walking = false;
        if (!lockWASDControls)
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
        anim.SetBool("Walking", walking);

        if (lastPos != transform.position)
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
}
