using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
	[SerializeField]
	private Transform cameraLocation;
	private void OnTriggerEnter(Collider c)
	{
		if(c.tag == "Player")
		{
			Debug.Log("Checkpoint Hit");
			GameManager.Instance.SetCheckpoint(this);
		}
	}
}
