using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IBreakable
{
	public GameObject brokenVersion;

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
}
