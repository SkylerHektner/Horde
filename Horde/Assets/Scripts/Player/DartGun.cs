using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DartGun : MonoBehaviour 
{
    public bool debugWindowOpen; // So player doesn't shoot when using the debug window.

    [SerializeField] private bool infiniteAmmo = false;

    [SerializeField] private GameObject dart;
    [SerializeField] private Material lineRendererMat;
    [SerializeField] private Transform dartSpawnLocation;
    [SerializeField] private Transform dartSpawnLocationCrouched;
    private Transform DartSpawn;

    [SerializeField] private float attackCooldown = 1;
    
    private Rigidbody dartRB;
	private int maxLaserDistance = 75;
	private Vector3 mousePosition;
	private RaycastHit gunRayHit;
    private bool attackOnCooldown = false;
    public bool isCrouching = false;

    public bool Paused { get; private set; }

    private LineRenderer lr;
    private PlayerMovement playerMovement;
    private Animator animator;

	private void Start() 
	{
		InitializeLineRenderer();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        PathosUI.instance.menuEvent.AddListener(pause);
	}
	
	private void Update() 
	{
        if (!playerMovement)
            playerMovement = GetComponent<PlayerMovement>();
        if (!animator)
            animator = GetComponent<Animator>();
        if (!lr)
            lr = GetComponent<LineRenderer>();

        if (playerMovement.isDead || Paused)
            return;
            
        if (isCrouching == true)
        {
            DartSpawn = dartSpawnLocationCrouched;
        }
        if (isCrouching == false)
        {
            DartSpawn = dartSpawnLocation;
        }

        if (Input.GetButton("Fire1") && !attackOnCooldown
            && (ResourceManager.Instance.CanSpendEmotion(PathosUI.instance.CurrentEmotion) || infiniteAmmo))
		{
            if(debugWindowOpen)
                return;

            playerMovement.lockMovementControls = true;
            animator.SetBool("Aiming", true);
            if (Input.GetButton("Crouch"))
            {
                animator.SetBool("Sneaking", true);
            }
            lr.enabled = true;
            if (Physics.Raycast(DartSpawn.position, transform.forward, out gunRayHit, maxLaserDistance)) // If the ray hits something.
            {
                if (gunRayHit.collider.gameObject.tag == "Enemy")
                    ChangeColor(Color.red);
                else
                    ChangeColor(Color.green);

                lr.SetPosition(0, DartSpawn.position);
                lr.SetPosition(1, gunRayHit.point);
            }
            else // If the ray doesn't hit anything, just render it's max distance.
            {
                ChangeColor(Color.green);

                lr.SetPosition(0, DartSpawn.position);
                lr.SetPosition(1, DartSpawn.position + transform.forward * maxLaserDistance);
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if(debugWindowOpen)
                return;
            playerMovement.lockMovementControls = false;
            animator.SetBool("Aiming", false);
            if (!attackOnCooldown)
                StartCoroutine(Fire());
            lr.enabled = false;
        }
    }

	private void InitializeLineRenderer()
	{
		lr = GetComponent<LineRenderer>(); 

		lr.positionCount = 2;
		lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.enabled = false;
        lr.material = lineRendererMat;
    }

	private void ChangeColor(Color c)
	{
		lr.startColor = c;
		lr.endColor = c;
	}

	private IEnumerator Fire()
	{
        // if the
        if (infiniteAmmo || ResourceManager.Instance.TrySpendEmotion(PathosUI.instance.CurrentEmotion))
        {
            attackOnCooldown = true;

            Vector3 endpoint = DartSpawn.position + transform.forward * maxLaserDistance;
            GameObject dartGO = Instantiate(dart, DartSpawn.position, transform.rotation);
            dartRB = dartGO.GetComponent<Rigidbody>();

            Vector3 directionalVector = endpoint - transform.position;
            directionalVector.y = 0;
            dartRB.velocity = directionalVector.normalized * 125;
            dartRB.rotation = Quaternion.LookRotation(dartRB.velocity);

            dartGO.GetComponent<Dart>().LoadEmotion(PathosUI.instance.CurrentEmotion);

            yield return new WaitForSeconds(attackCooldown);

            attackOnCooldown = false;
        }
        else
        {
            yield return null;
        }
	}
    
    private void pause(bool paused)
    {
        Paused = paused;
    }
}
