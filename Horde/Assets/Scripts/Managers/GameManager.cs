﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Room CurrentRoom { get { return currentRoom; } }
    public Player Player { get { return player; } }
    public bool PlayerIsMarked { get { return playerIsMarked; } set { playerIsMarked = value; } }
    public float OutOfVisionDuration { get { return outOfVisionDuration; } set { outOfVisionDuration = value; } }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private List<Room> rooms;
    [SerializeField] private FadeCamera fadeCamera;

    private Room currentRoom;
    private Room lastCheckpoint;

    // Helpers to give guards shared vision during alert state. //
    private Player player;
    private bool playerIsMarked;
    private float outOfVisionDuration; // The amount of time the current target has been out of vision.

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        currentRoom = rooms[0];
        lastCheckpoint = rooms[0];

        Debug.Log(currentRoom.CameraSpawn.position);
        cameraController.MoveTo(currentRoom.CameraSpawn);
    }

    private void Update()
    {
        OutOfVisionDuration += Time.smoothDeltaTime;

        // Lock the door if guards are alerted.
        if (playerIsMarked)
            currentRoom.Exit.LockDoor();
        else
            currentRoom.Exit.UnlockDoor();
        if(fadeCamera.isBlack)
            Debug.Log(fadeCamera.isBlack);

    }

    /// <summary>
    /// Alerts all the guards in the room.
    /// </summary>
    public void AlertGuards()
    {
        Debug.Log(currentRoom.Enemies.Count);
        Debug.Log(currentRoom.RoomName);
        foreach (Enemy e in currentRoom.Enemies)
        {
            if (e.GetCurrentState() is Idle || e.GetCurrentState() is Patrol)
                e.ChangeState(new Alert(e));
        }
    }

    public void TransitionToNextRoom()
    {
        
        Room nextRoom;
        if (rooms[rooms.Count - 1] == currentRoom) // If it's the last room.
            nextRoom = currentRoom; // Just loop in same room for now.
        else
            nextRoom = rooms[rooms.IndexOf(currentRoom) + 1];
        
        currentRoom = nextRoom;
        // The disabling of the room looks very jarring to the player. I think
        // we should leave it enabled. - Skyler
        //currentRoom.gameObject.SetActive(true); // Activate the new room.
        //rooms[rooms.IndexOf(currentRoom) - 1].gameObject.SetActive(false);

        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;
        
        StartCoroutine(cameraPause(currentRoom));
        fadeCamera.Reset();

    }
    public IEnumerator cameraPause(Room currentRoom)
    {
        Debug.Log(fadeCamera.isBlack);
        yield return new WaitUntil(() => fadeCamera.isBlack);
        fadeCamera.isBlack = false;
        Debug.Log("passed WaitUntil");
        Debug.Log(currentRoom.CameraSpawn);
        cameraController.MoveTo(currentRoom.CameraSpawn);
    }
}
