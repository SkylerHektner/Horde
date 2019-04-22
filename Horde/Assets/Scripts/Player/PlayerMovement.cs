using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float crouchSpeed = 1f;

    public BoxCollider StandingHitbox;
    public BoxCollider CrouchingHitbox;

    public bool lockMovementControls = false;
    public bool isDead = false;
    public bool Paused { get; set; }

    private Vector3 forward;
    private Vector3 right;

    private NavMeshAgent agent;
    private Animator anim;

    private Ray cameraRay;
	private RaycastHit cameraRayHit;

    private int layerMask = 1 << 9; // Layer mask for the background.

    //private const float ROOT2 = 0.707f;

    private GameObject mainCamera;
    private GameObject cameraTransformGO;
    private Transform cameraTransform;

    private Vector3 lastPos;
	void Start ()
    {
        speed = GameManager.Instance.Player.PlayerSettings.MovementSpeed;
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTransformGO = new GameObject();
        cameraTransform = cameraTransformGO.transform;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;

        PathosUI.instance.menuEvent.AddListener(pause);
    }
	
	void Update ()
    {
        cameraTransform.position = mainCamera.transform.position;
        cameraTransform.rotation = Quaternion.Euler(0, 
                                                    mainCamera.transform.rotation.eulerAngles.y, 
                                                    mainCamera.transform.rotation.eulerAngles.z);

        forward = new Vector3(cameraTransform.forward.x * speed, 0, cameraTransform.forward.z * speed);
        right = new Vector3(cameraTransform.right.x * speed, 0, cameraTransform.right.z * speed);
        
        // no movement if paused
        if(isDead || Paused)
            return;

        // allow movement is movement is not locked
        if (!lockMovementControls)
        {
            Vector3 dest = Vector3.zero;
            dest += forward * (Time.deltaTime * crouchSpeed) * Input.GetAxis("Vertical");
            dest += right * (Time.deltaTime * crouchSpeed) * Input.GetAxis("Horizontal");
            agent.Move(dest);
        }

        bool moving = transform.position != lastPos;

        // Make the player face in the direction of the mouse position when not moving
        if (!moving)
        {
            cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out cameraRayHit, float.MaxValue, layerMask))
            {
                Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                transform.forward = Vector3.Lerp(transform.forward, targetPosition - transform.position, Time.deltaTime * 2f);
            }
            if (Input.GetAxis("Aim Horizontal") != 0f || Input.GetAxis("Aim Vertical") != 0f)
            {
                Vector3 look = Vector3.zero;
                look -= forward * (Time.deltaTime * crouchSpeed) * Input.GetAxis("Aim Vertical");
                look += right * (Time.deltaTime * crouchSpeed) * Input.GetAxis("Aim Horizontal");
                transform.forward = look;
            }
        }

        // when they are moving make them face the direction of the movement with a smoothing lerp
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

        // Controls for crouching
        if (Input.GetButtonDown("Crouch"))
        {
            crouchSpeed = .5f;
            anim.SetBool("Sneaking", true);
            agent.height = 0.33f;
            StandingHitbox.enabled = false;
            CrouchingHitbox.enabled = true;
            GetComponent<DartGun>().isCrouching = true;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            crouchSpeed = 1f;
            anim.SetBool("Sneaking", false);
            agent.height = 1.61f;
            StandingHitbox.enabled = true;
            CrouchingHitbox.enabled = false;
            GetComponent<DartGun>().isCrouching = false;

        }
    }

    private void pause(bool paused)
    {
        Paused = paused;
        anim.enabled = !paused;
        agent.isStopped = paused;
    }

    public void LookAt(Vector3 pos)
    {
		Vector3 direction = pos - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        //Transform head = GetComponentInChildren<VisionCone>().transform;
		transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 5.0f * Time.deltaTime);
    }
}
