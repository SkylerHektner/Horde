using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLaser : MonoBehaviour 
{
	private int maxLaserDistance = 50;

	private Vector3 mousePosition;
	private LineRenderer lr;
	private RaycastHit gunRayHit;

	void Start () 
	{
		lr = GetComponent<LineRenderer>();

		lr.positionCount = 2;

		lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.material = new Material(Shader.Find("Particles/Additive"));
	}

	void Update () 
	{
		// Set the origin of the ray.
		Vector3 origin = new Vector3(transform.position.x, 7, transform.position.z);

		//Debug.DrawRay(origin, transform.forward * maxLaserDistance, Color.red);

		if (Physics.Raycast(origin, transform.forward, out gunRayHit, maxLaserDistance)) // If the ray hits something.
		{
			if(gunRayHit.collider.gameObject.tag == "TeamTwoUnit")
				ChangeColor(Color.red);
			else
				ChangeColor(Color.green);

			lr.SetPosition(0, origin);
			lr.SetPosition(1, gunRayHit.point);
		}
		else // If the ray doesn't hit anything, just render it's max distance.
		{
			ChangeColor(Color.green);	

			lr.SetPosition(0, origin);
			lr.SetPosition(1, origin + transform.forward * maxLaserDistance);
		}
	}

	private void ChangeColor(Color c)
	{
		lr.startColor = c;
		lr.endColor = c;
	}
}
