using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour 
{
	public PlayerSettings PlayerSettings { get { return playerSettings; } }
	public Vector3 SpawnLocation { get { return spawnLocation; } }

	[SerializeField] private PlayerSettings playerSettings;

	private Vector3 spawnLocation;
	
	void Start () 
	{
		SetKinematic(true);
		spawnLocation = transform.position;
	}
	
	public void Respawn()
	{
		GetComponent<NavMeshAgent>().Warp(spawnLocation);
		GetComponent<PlayerMovement>().lockMovementControls = false;
	}

	public void Die()
	{
		GetComponent<Animator>().SetTrigger("Die");
		SetKinematic(false);
		GetComponent<Animator>().enabled = false;
		GetComponent<Collider>().enabled = false;
	}

	// Used for the ragdoll rigidbodies.
	private void SetKinematic(bool value)
	{
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic = value;
		}
	}
}
