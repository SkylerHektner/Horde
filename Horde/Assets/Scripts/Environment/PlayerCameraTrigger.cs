using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTrigger : MonoBehaviour 
{
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			c.gameObject.GetComponent<PlayerMovement>().lockCamToPlayer = true;
		}
	}
}
