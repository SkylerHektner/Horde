using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Room CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }
    public List<Room> Rooms {get {return rooms; } }

    public bool RoomIsAlerted { get { return roomIsAlerted; } set { roomIsAlerted = value; } }
    public Player Player { get { return player; } }
    public bool PlayerIsMarked { get { return playerIsMarked; } set { playerIsMarked = value; } }
    public float OutOfVisionDuration { get { return outOfVisionDuration; } set { outOfVisionDuration = value; } }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private FadeCamera fadeCamera;
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Transform roomNamePopup;

    private Room currentRoom;

    // Helpers to give guards shared vision during alert state. //
    private bool roomIsAlerted;
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

        // Deactivate all the rooms. (Optimization)
        foreach(Room r in rooms)
        {
            r.gameObject.SetActive(false);
        }

        // Get a reference to the player. (Helpful for the shared vision of guards)
        player = FindObjectOfType<Player>();

        Room currentCheckpoint = rooms[PlayerPrefs.GetInt("Checkpoint")];
        ResourceManager.Instance.Rage = PlayerPrefs.GetInt("Anger", 0);
        ResourceManager.Instance.Fear = PlayerPrefs.GetInt("Fear", 0);
        ResourceManager.Instance.Sadness = PlayerPrefs.GetInt("Sadness", 0);
        ResourceManager.Instance.Joy = PlayerPrefs.GetInt("Joy", 0);

        if(currentCheckpoint == null)
            currentRoom = rooms[0]; // Just start in the first room is there is no current checkpoint.
        else
            currentRoom = currentCheckpoint;

        currentRoom.gameObject.SetActive(true); // Activate the new room.

        // Spawn the player in the correct position and rotation in the room.
        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;

        // Move the camera to the correct position and rotation also.
        cameraController.MoveTo(currentRoom.CameraSpawn);
    }

    private void Update()
    {
        OutOfVisionDuration += Time.smoothDeltaTime;

        // Lock the door if guards are alerted.
        if (roomIsAlerted)
        {
            currentRoom.Exit.LockDoor();

            // Edge case to handle when there is 1 guard remaining and a dart changes him out of alert state.
            if(currentRoom.Enemies.Count == 1)
            {
                if(!(currentRoom.Enemies[0].GetCurrentState() is Alert)) // If the last enemy isn't in an Alert state.
                {
                    roomIsAlerted = false;
                    currentRoom.Exit.UnlockDoor();
                }
            }
        }  
        else if (currentRoom != null)
            currentRoom.Exit.UnlockDoor();
    }

    /// <summary>
    /// Alerts all the guards in the room.
    /// </summary>
    public void AlertGuards()
    {
        foreach (Enemy e in currentRoom.Enemies)
        {
            if (e.GetCurrentState() is Idle || e.GetCurrentState() is Patrol || e.GetCurrentState() is Alert)
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

        currentRoom.gameObject.SetActive(true); // Activate the new room.
        rooms[rooms.IndexOf(currentRoom) - 1].gameObject.SetActive(false); // Deactivate previous room.

        if(currentRoom.IsCheckpoint)
        {
            // Set the room as the current checkpoint.
            PlayerPrefs.SetInt("Checkpoint", rooms.IndexOf(currentRoom)); 

            // Save the resources the player has when reaching the checkpoint.
            PlayerPrefs.SetInt("Anger", ResourceManager.Instance.Rage);
            PlayerPrefs.SetInt("Fear", ResourceManager.Instance.Fear);
            PlayerPrefs.SetInt("Sadness", ResourceManager.Instance.Sadness);
            PlayerPrefs.SetInt("Joy", ResourceManager.Instance.Joy);
        }

        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;

        cameraController.MoveTo(currentRoom.CameraSpawn);
        StartCoroutine(DisplayRoomName());
    }

    public Enemy GetClosestGuardToPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        Enemy closestGuard = currentRoom.Enemies[0];
        float closestDistance = Mathf.Infinity;

        foreach (Enemy guard in currentRoom.Enemies)
        {
            if(guard == null) // A guard could have been destroyed.
                continue;

            // Calculate the closest guard to the last seen player location.
            NavMesh.CalculatePath(guard.transform.position, VisionCone.LastSeenPlayerLocation, NavMesh.AllAreas, path);

            if(path.status == NavMeshPathStatus.PathComplete) // Make sure it's a valid path.
            {
                float distance = GetPathDistance(path.corners);

                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestGuard = guard;
                }
            }
        }

        return closestGuard;
    }

    private IEnumerator DisplayRoomName()
    {
        Transform instance = Instantiate(roomNamePopup);
        instance.SetParent(PathosUI.instance.transform);
        instance.GetComponent<TextMeshProUGUI>().SetText(currentRoom.RoomName);

        yield return new WaitForSeconds(3.0f);

        Destroy(instance.gameObject);
    }

    /// <summary>
    /// Given an array of Vector3's (the corners of the path),
    /// This function will return the total distance of the path.
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    private float GetPathDistance(Vector3[] corners)
    {
        float totalDistance = 0;

        for(int i = 0; i < corners.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(corners[i], corners[i + 1]);
        }

        return totalDistance;
    }
}
