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
		spawnLocation = transform.position;
	}
	
	public void Respawn()
	{
		GetComponent<NavMeshAgent>().Warp(spawnLocation);
		GetComponent<PlayerMovement>().lockMovementControls = false;
	}
}
