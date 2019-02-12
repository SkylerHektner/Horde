using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private float crouchSpeed = 1f;
    [SerializeField]
    private CameraController cam;

    public GameObject Backpack;
    public GameObject North;
    public Vector3 CamGirl;

    public BoxCollider StandingHitbox;
    public BoxCollider CrouchingHitbox;

    public bool lockCamToPlayer = true;
    public bool lockToBack = false;
    public bool lockMovementControls = false;
    public bool isDead = false;

    private Vector3 forward;
    private Vector3 right;

    private NavMeshAgent agent;
    private Animator anim;

    private Ray cameraRay;
	private RaycastHit cameraRayHit;

    private int layerMask = 1 << 9; // Layer mask for the background.

    private const float ROOT2 = 0.707f;

    private Vector3 lastPos;
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        forward = new Vector3(-ROOT2 * speed, 0, ROOT2 * speed);
        right = new Vector3(ROOT2 * speed, 0, ROOT2 * speed);
        anim = GetComponent<Animator>();
        lastPos = transform.position;
    }
	
	void Update ()
    {
        if(isDead)
            return;

        // if we are not being carried and our controls are not locked, try to detect movement input and move
        if (!lockMovementControls)
        {
            Vector3 dest = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                //agent.Move(forward * Time.deltaTime);
                //agent.SetDestination(transform.position + forward);
                dest += forward * (Time.deltaTime * crouchSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //agent.Move(forward * -Time.deltaTime);
                //agent.SetDestination(transform.position + -forward);
                dest -= forward * (Time.deltaTime * crouchSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                //agent.Move(right * -Time.deltaTime);
                //agent.SetDestination(transform.position + -right);
                dest -= right * (Time.deltaTime * crouchSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //agent.Move(right * Time.deltaTime);
                //agent.SetDestination(transform.position + right);
                dest += right * (Time.deltaTime * crouchSpeed);
            }
            agent.Move(dest);
        }

        bool moving = transform.position != lastPos;

        // Make the player face in the direction of the mouse position when not moving
        if(!moving)
        {
            cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out cameraRayHit, float.MaxValue, layerMask) && lockToBack == false)
            {
                Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                if (!lockToBack)
                {
                    transform.forward = Vector3.Lerp(transform.forward, targetPosition - transform.position, Time.deltaTime * 2f);
                }
            }
            if (Physics.Raycast(cameraRay, out cameraRayHit, float.MaxValue, layerMask) && lockToBack == true)
            {
                Vector3 targetPosition = new Vector3(North.transform.position.x, North.transform.position.y, North.transform.position.z);
                transform.forward = Vector3.Lerp(transform.forward, targetPosition - transform.position, Time.deltaTime * 2f);

            }
        }

        if (moving)
        {
            transform.forward = Vector3.Lerp(transform.forward, (transform.position - lastPos).normalized, Time.deltaTime * 7f).normalized;
            lastPos = transform.position;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (lockCamToPlayer)
        {
            if (!lockToBack)
            {
                Vector3 pos = new Vector3(transform.position.x - 9f, transform.position.y + 50f, transform.position.z + 9f);
                cam.SetTargetPos(pos);
            }


            if (lockToBack)
            {
                Vector3 position = new Vector3(Backpack.transform.position.x - 23f, Backpack.transform.position.y + 27f, Backpack.transform.position.z +23);
                cam.SetTargetPos(position);
            }
        }

        if (!lockCamToPlayer)
        {

            if (!lockToBack)
            {
                Vector3 pos = CamGirl;
                cam.SetTargetPos(pos);
            }


            if (lockToBack)
            {
                Vector3 position = new Vector3(Backpack.transform.position.x - 23f, Backpack.transform.position.y + 27f, Backpack.transform.position.z + 23f);
                cam.SetTargetPos(position);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            crouchSpeed = .5f;
            GetComponent<Animator>().SetBool("Sneaking", true);
            GetComponent<NavMeshAgent>().height = 0.63f;
            StandingHitbox.enabled = false;
            CrouchingHitbox.enabled = true;
            GetComponent<DartGun>().isCrouching = true;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            crouchSpeed = 1f;
            GetComponent<Animator>().SetBool("Sneaking", false);
            GetComponent<NavMeshAgent>().height = 1.61f;
            StandingHitbox.enabled = true;
            CrouchingHitbox.enabled = false;
            GetComponent<DartGun>().isCrouching = false;

        }
    }
}
