using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DartGun : MonoBehaviour 
{

    [SerializeField] private GameObject dart;
    [SerializeField] private Transform dartSpawnLocation;
    [SerializeField] private Transform dartSpawnLocationCrouched;
    private Transform DartSpawn;

    [SerializeField] private float attackCooldown = 1;
    
    private Rigidbody dartRB;
	private int maxLaserDistance = 75;
	private Vector3 mousePosition;
	private LineRenderer lr;
	private RaycastHit gunRayHit;
    private bool attackOnCooldown = false;
    public bool isCrouching = false;

	private void Start() 
	{
		InitializeLineRenderer();
	}
	
	private void Update() 
	{
        if (isCrouching == true)
        {
            DartSpawn = dartSpawnLocationCrouched;
        }
        if (isCrouching == false)
        {
            DartSpawn = dartSpawnLocation;
        }

        if (Input.GetMouseButton(0) && GetComponent<PlayerMovement>().lockToBack == false && !attackOnCooldown)
		{
            GetComponent<PlayerMovement>().lockMovementControls = true;
            GetComponent<Animator>().SetBool("Aiming", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<Animator>().SetBool("Sneaking", true);
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

        if (Input.GetMouseButtonUp(0) && GetComponent<PlayerMovement>().lockToBack == false)
        {
            GetComponent<PlayerMovement>().lockMovementControls = false;
            GetComponent<Animator>().SetBool("Aiming", false);
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
        lr.material = new Material(Shader.Find("Particles/Additive"));
        lr.enabled = false;
	}

	private void ChangeColor(Color c)
	{
		lr.startColor = c;
		lr.endColor = c;
	}

	private IEnumerator Fire()
	{
        attackOnCooldown = true;

		Vector3 endpoint = DartSpawn.position + transform.forward * maxLaserDistance;
		GameObject dartGO = Instantiate(dart, DartSpawn.position, transform.rotation);
		dartRB = dartGO.GetComponent<Rigidbody>();

		Vector3 directionalVector = endpoint - transform.position;
		directionalVector.y = 0;
		dartRB.velocity = directionalVector.normalized * 125;
		dartRB.rotation = Quaternion.LookRotation(dartRB.velocity);

        ResourceManager.Instance.SpendEmotion(PathosUI.instance.CurrentEmotion);

		dartGO.GetComponent<Dart>().LoadEmotion(PathosUI.instance.CurrentEmotion);

        yield return new WaitForSeconds(attackCooldown);

        attackOnCooldown = false;
	}
}
