using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCameraTrigger : MonoBehaviour 
{

	[SerializeField]
	private Transform cameraLocation;
	private void OnTriggerEnter(Collider c)
	{
		if(c.tag == "Player")
		{
			c.GetComponent<PlayerMovement>().lockCamToPlayer = false;
			GameManager.Instance.SetCameraLocation(cameraLocation);
		}
	}
}
