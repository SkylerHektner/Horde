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
    private bool inTargetingAction = false;
    public bool isDead = false;

    private bool beingCarried = false;
    [SerializeField]
    private YesNoDialogue yesNoDioPrefab;
    private YesNoDialogue curDio;

    private Vector3 forward;
    private Vector3 right;

    private NavMeshAgent agent;
    private Animator anim;

    private Ray cameraRay;
	private RaycastHit cameraRayHit;

    private int layerMask = 1 << 9; // Layer mask for the background.

    public MovementPattern movementPattern = MovementPattern.WASD;
    public enum MovementPattern
    {
        WASD,
        ClickToMove
    }

    private const float ROOT2 = 0.707f;

    private Vector3 lastPos;
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        forward = new Vector3(-ROOT2 * speed, 0, ROOT2 * speed);
        right = new Vector3(ROOT2 * speed, 0, ROOT2 * speed);
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        //HTargetingTool.OnTargeting += OnTargetingAction;
        //HTargetingTool.OnFinishedTargeting += OnFinishedTargeting;
    }
	
	void Update ()
    {

        // Make the player face in the direction of the mouse position.
		cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(cameraRay, out cameraRayHit, float.MaxValue, layerMask) && lockToBack == false && isDead == false)
		{
			Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);

            if (!lockToBack)
            {
                transform.LookAt(targetPosition);
            }

        }
        if (Physics.Raycast(cameraRay, out cameraRayHit, float.MaxValue, layerMask) && lockToBack == true)
        {
            Vector3 targetPosition = new Vector3(North.transform.position.x, North.transform.position.y, North.transform.position.z);
            transform.LookAt(targetPosition);

        }
        if (lockMovementControls) // if caught
        {
            //GetComponent<Animator>().SetTrigger("Caught");
        }

        // if we are not being carried and our controls are not locked, try to detect movement input and move
        if (!lockMovementControls && !beingCarried)
        {
            if(movementPattern == MovementPattern.WASD)
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
            else if (movementPattern == MovementPattern.ClickToMove)
            {
                RaycastHit hitinfo;
                if (Input.GetMouseButton(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo))
                {
                    agent.SetDestination(hitinfo.point);
                }
            }
        }
        // If they are being carried and try to move ask them if they want to stop being carried
        else if (beingCarried && !inTargetingAction)
        {
            if (((movementPattern == MovementPattern.WASD) && 
                (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) 
                ||
                ((movementPattern == MovementPattern.ClickToMove) && Input.GetMouseButtonDown(1)))
            {
                if (curDio == null)
                {
                    curDio = Instantiate(yesNoDioPrefab);
                    curDio.Init(untoggleCarryMode, null, "Would you like to stop being carried?");
                }
            }
        }
        

        if (lastPos != transform.position && !beingCarried)
        {
            transform.forward = transform.position - lastPos;
            lastPos = transform.position;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Vector3 temp = forward;
        //    forward = right;
        //    right = -temp;
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Vector3 temp = forward;
        //    forward = -right;
        //    right = temp;
        //}

        if (lockCamToPlayer)
        {
            if (!lockToBack)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 60f, transform.position.z);
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
            GetComponent<NavMeshAgent>().height = 0.83f;
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

    /// <summary>
    /// call this to set the player in carry mode.
    /// Disabled movement and makes a prompt if they try to move
    /// </summary>
    public void toggleCarryMode()
    {
        lockMovementControls = true;
        beingCarried = true;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    public void untoggleCarryMode()
    {
        transform.parent = null;
        lockMovementControls = false;
        beingCarried = false;
        GetComponent<NavMeshAgent>().enabled = true;
    }

    private void OnTargetingAction()
    {
        lockCamToPlayer = false;
        lockMovementControls = true;
        inTargetingAction = true;
    }

    private void OnFinishedTargeting()
    {
        lockCamToPlayer = true;
        lockMovementControls = false;
        inTargetingAction = false;
    }
}
