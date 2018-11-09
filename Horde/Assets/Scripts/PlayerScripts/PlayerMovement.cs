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

    private Vector3 forward;
    private Vector3 right;

    private NavMeshAgent agent;
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        forward = new Vector3(0, 0, speed);
        right = new Vector3(speed, 0, 0);
    }
	
	void Update ()
    {
        if (ClassEditorUI.Instance.InEditMode)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            agent.Move(forward * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            agent.Move(forward * -Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            agent.Move(right * -Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            agent.Move(right * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 temp = forward;
            forward = right;
            right = -temp;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 temp = forward;
            forward = -right;
            right = temp;
        }

        cam.SetTargetPos(transform.position.x, transform.position.z);
    }
}
