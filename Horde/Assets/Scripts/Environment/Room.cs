using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour 
{
	public List<Enemy> Enemies { get { return enemies; } }

	[SerializeField] private string roomName; // Not sure if we need this or not.

	private List<Enemy> enemies;
	private Checkpoint checkpoint;

	
	void Start () 
	{
		InitializeVariables(); // Initialize enemies, checkpoint, etc...
		CheckIfProperlyInitialized(); // Checks for initialization errors, mainly null values.
	}
	
	
	void Update () 
	{
		
	}

	public void RespawnRoom()
	{
		foreach(Enemy e in enemies)
		{
			e.Respawn();
		}
	}

	private void InitializeVariables()
	{
		enemies = new List<Enemy>();
		
		foreach(Enemy e in GetComponentsInChildren<Enemy>())
		{
			enemies.Add(e);	// Populate the enemies list.
		}

		checkpoint = GetComponentInChildren<Checkpoint>(); // Set the checkpoint.
	}

	private void CheckIfProperlyInitialized()
	{
		if(enemies.Count == 0)
			Debug.LogWarning("There are no enemies in this room. Is this intentional?");

		if(checkpoint == null)
			Debug.LogError("Please set a checkpoint for the room");
	}
}
