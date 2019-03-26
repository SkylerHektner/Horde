using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IBreakable 
{
	public Transform Transform { get { return transform; } } 
	
	public void Break()
	{
		Destroy(this.gameObject); // Temporary.
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}
}
