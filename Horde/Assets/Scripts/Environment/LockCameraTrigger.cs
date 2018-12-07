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
            Vector3 newPos = new Vector3(cameraLocation.position.x, cameraLocation.position.y + 60f, cameraLocation.position.z);
            //cameraLocation.position = newPos;
           
			GameManager.Instance.SetCameraLocation(newPos);
		}
	}
}
