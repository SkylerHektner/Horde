using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DartGun : MonoBehaviour 
{
	[SerializeField]
	private GameObject dart;

	[SerializeField]
	private Transform dartSpawnLocation;

	private Rigidbody dartRB;
	private int maxLaserDistance = 75;
	private Vector3 mousePosition;
	private LineRenderer lr;
	private RaycastHit gunRayHit;

	private void Start() 
	{
		InitializeLineRenderer();
	}
	
	private void Update() 
	{
		if (Input.GetMouseButton(0))
		{
            lr.enabled = true;
            if (Physics.Raycast(dartSpawnLocation.position, transform.forward, out gunRayHit, maxLaserDistance)) // If the ray hits something.
            {
                if (gunRayHit.collider.gameObject.tag == "Enemy")
                    ChangeColor(Color.red);
                else
                    ChangeColor(Color.green);

                lr.SetPosition(0, dartSpawnLocation.position);
                lr.SetPosition(1, gunRayHit.point);
            }
            else // If the ray doesn't hit anything, just render it's max distance.
            {
                ChangeColor(Color.green);

                lr.SetPosition(0, dartSpawnLocation.position);
                lr.SetPosition(1, dartSpawnLocation.position + transform.forward * maxLaserDistance);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
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

	public void Fire()
	{
		Vector3 endpoint = dartSpawnLocation.position + transform.forward * maxLaserDistance;
		GameObject dartGO = Instantiate(dart, dartSpawnLocation.position, transform.rotation);
		dartRB = dartGO.GetComponent<Rigidbody>();

		Vector3 directionalVector = endpoint - transform.position;
		directionalVector.y = 0;
		dartRB.velocity = directionalVector.normalized * 125;
		dartRB.rotation = Quaternion.LookRotation(dartRB.velocity);

        ResourceManager.Instance.SpendEmotion(PathosUI.instance.CurrentEmotion);

		dartGO.GetComponent<Dart>().LoadEmotion(PathosUI.instance.CurrentEmotion);

		//Debug.Log(dart.GetComponent<Dart>().LoadedEmotion);
		//Debug.Log(PathosUI.instance.CurrentEmotion);
	}
}
