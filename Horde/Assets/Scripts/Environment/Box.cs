using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IBreakable
{
	public GameObject brokenVersion;
	
	private float distToGround;
	private LayerMask mask;
	private LayerMask rayCastPlane;
	private Rigidbody rb;

	private void Start()
	{
		distToGround = GetComponentInChildren<Collider>().bounds.extents.y;

		mask = 1 << LayerMask.NameToLayer("Ground");
		rayCastPlane = 1 << LayerMask.NameToLayer("RaycastPlane");
		mask = mask | ~rayCastPlane;
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if(IsFloating())
			rb.isKinematic = false;
		else
			StartCoroutine(ChangeToKinematic()); // We need to use a coroutine to give it an extra 1/10 of a second
												 // before we set kinematic back to true or else it sinks into the ground.
	}

	public void Break()
	{
		GameObject go = Instantiate(brokenVersion, transform.position, transform.rotation);
		foreach(Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
		{
			rb.GetComponent<Rigidbody>().AddExplosionForce(1.0f, transform.position, 5.0f, 3.0f, ForceMode.Impulse);
		}
		
		Destroy(gameObject);
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	private bool IsFloating()
	{
		RaycastHit hit;
		Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, mask);
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);

		//Debug.Log(hit.distance);
		//Debug.Log(distToGround);

		Debug.Log(hit.distance - distToGround);

		if(hit.distance - distToGround > 0.1f)
			return true;
		else
			return false;
	}

	private IEnumerator ChangeToKinematic()
	{
		yield return new WaitForSeconds(0.1f);

		rb.isKinematic = true;
	}
}
