using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour 
{
	public List<Enemy> Enemies { get { return enemies; } }
	public string RoomName { get { return roomName; } }
	public Exit Exit { get { return exit; } }
	public Transform Spawn { get { return playerSpawn; } }
	public Transform CameraSpawn { get { return cameraSpawn; } }

	[SerializeField] private string roomName;
	[SerializeField] private bool isCheckpoint; // Is a checkpoint room.

	private List<Enemy> enemies;
	private Exit exit;
	private Transform playerSpawn;
	private Transform cameraSpawn;

	
	void Awake() 
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

		playerSpawn = transform.Find("Player-Spawn");
		cameraSpawn = transform.Find("Camera-Spawn");
		exit = GetComponentInChildren<Exit>();
	}

	private void CheckIfProperlyInitialized()
	{
		if(enemies.Count == 0)
			Debug.LogError("There are no enemies detected in room: " + roomName);

		if(playerSpawn == null)
			Debug.LogError("There is no Player Spawn in room: " + roomName);
		
		if(cameraSpawn == null)
			Debug.LogError("There is no Camera Spawn in room: " + roomName);

		if(exit == null)
			Debug.LogError("There is no exit detected in room: " + roomName);
	}
}
