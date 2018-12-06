using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour 
{
	private List<HInterface.HType> heuristics;
	public List<HInterface.HType> Heuristics { get { return heuristics; }  set { heuristics = value; } }

	private Rigidbody rb;
	private Vector3 oldVel;
	private int bounces = 0;

	void Start () 
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () 
	{ 
		// Rotate the projectile along it's projectile path.
		transform.rotation = Quaternion.LookRotation(rb.velocity);
		oldVel = rb.velocity;
	}

	void OnCollisionEnter(Collision c)
	{
		if(c.gameObject.tag == "TeamTwoUnit")
		{
			//Debug.Log("Hit team two unit");
			
			c.gameObject.GetComponent<Unit>().OverrideHeuristics(heuristics);

			Destroy(gameObject);
		}

		bounces++;
		if(bounces >= 3)
		{
			Destroy(gameObject);
		}

		ContactPoint cp = c.contacts[0];
		rb.velocity = Vector3.Reflect(oldVel, cp.normal).normalized * 125;
	}
}
