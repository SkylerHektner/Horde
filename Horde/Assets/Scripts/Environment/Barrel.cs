using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IBreakable 
{
	public void Break()
	{
		Destroy(this.gameObject); // Temporary.
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}
}
