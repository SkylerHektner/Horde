using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public UnityEvent ChangeRoomEvent;

    public bool RoomIsAlerted           { get { return roomIsAlerted; } set { roomIsAlerted = value; } }
    public Room CurrentRoom             { get { return currentRoom; } set { currentRoom = value; } }
    public List<Room> Rooms             { get {return rooms; } }
    public Player Player                { get { return player; } }
    public GameSettings GameSettings    { get { return gameSettings; } }

    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private FadeCamera fadeCamera;
    [SerializeField] private List<Room> rooms;              // A list of all the rooms in the current scene.
    [SerializeField] private Transform roomNamePopup;       // A reference to the UI popup during a room transition.

    private Room currentRoom;
    private Room currentCheckpoint;
    private bool roomIsAlerted;
    private Player player;
    private int currentEpisode;

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

        // Get a reference to the player.
        player = FindObjectOfType<Player>();

        // Get the current checkpoint and activate that room.
        int roomIndex = PlayerPrefs.GetInt("Checkpoint", 0);
        if(roomIndex >= rooms.Count)
            currentCheckpoint = rooms[0];
        else
            currentCheckpoint = rooms[roomIndex];

        // If it's the first room of level 1, pause the game for 10 seconds to allow an animation to play.
        currentEpisode = PlayerPrefs.GetInt("Episode", 1);
        if(currentEpisode == 1 && roomIndex == 0)
        {
            player.GetComponent<Animator>().SetTrigger("StartMission");
            StartCoroutine(PausePlayerForSeconds(8.5f));
        }

            
        currentRoom = currentCheckpoint;
        currentRoom.gameObject.SetActive(true);

        // Spawn the player in the correct position and rotation in the room.
        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;

        // Move the camera to the correct position and rotation also.
        cameraController.MoveTo(currentRoom.CameraSpawn);
    }

    private void Start()
    {
        ResourceManager.Instance.Rage = PlayerPrefs.GetInt("Anger", 0);
        ResourceManager.Instance.Fear = PlayerPrefs.GetInt("Fear", 0);
        ResourceManager.Instance.Sadness = PlayerPrefs.GetInt("Sadness", 0);
        ResourceManager.Instance.Joy = PlayerPrefs.GetInt("Joy", 0);
        ChangeRoomEvent = new UnityEvent();
    }

    private void Update()
    {
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
        else if (currentRoom != null && currentRoom != rooms[rooms.Count - 1])
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

    public void TransitionToNextEpisode()
    {
        // Set the checkpoint to 0 so player spawns in first room of next episode.
        PlayerPrefs.SetInt("Checkpoint", 0); 

        // Clear all the resources
        PlayerPrefs.DeleteKey("Anger");
        PlayerPrefs.DeleteKey("Fear");
        PlayerPrefs.DeleteKey("Sadness");

        // Set the current episode to the next episode.
        int currentEpisode = PlayerPrefs.GetInt("Episode", 1);
        PlayerPrefs.SetInt("Episode", currentEpisode + 1);

        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Episode", 1));
    }

    /// <summary>
    /// Transitions the player into the next room after arriving at the exit of the previous room.
    /// The next room is the subsequent room in the rooms list.
    /// - The player gets moved to the spawn location of the next room.
    /// - The camera pans to the next room.
    /// - If the next room is a checkpoint, the player's current resources get saved to player prefs.
    /// </summary>
    public void TransitionToNextRoom()
    {
        Room nextRoom;
        if (rooms[rooms.Count - 1] == currentRoom)  // If it's the last room.
            nextRoom = rooms[0];                    // Just loop to the beginning for now.
        else
            nextRoom = rooms[rooms.IndexOf(currentRoom) + 1];
        
        currentRoom = nextRoom;

        currentRoom.gameObject.SetActive(true);                             // Activate the new room.
        rooms[rooms.IndexOf(currentRoom) - 1].gameObject.SetActive(false);  // Deactivate previous room.

        // If the next room is a checkpoint, update the current checkpoint and save the current resources (PlayerPrefs).
        if(currentRoom.IsCheckpoint)
        {
            PlayerPrefs.SetInt("Checkpoint", rooms.IndexOf(currentRoom)); 

            PlayerPrefs.SetInt("Anger", ResourceManager.Instance.Rage);
            PlayerPrefs.SetInt("Fear", ResourceManager.Instance.Fear);
            PlayerPrefs.SetInt("Sadness", ResourceManager.Instance.Sadness);
            PlayerPrefs.SetInt("Joy", ResourceManager.Instance.Joy);
        }

        // Teleport the player into the next room and pan the camera.
        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;
        cameraController.MoveTo(currentRoom.CameraSpawn);

        // Make UI popup displaying "Checkpoint Reached" only if the next room is a checkpoint.
        if(currentRoom.IsCheckpoint)
            PathosUI.instance.CheckpointNotif.GetComponent<Animation>().Play();
        //StartCoroutine(DisplayCheckpointPopup());

        ChangeRoomEvent.Invoke();
    }

    // Probably only used for debugging.
    public void TransitionToPreviousRoom()
    {
        Room nextRoom;
        if (rooms[0] == currentRoom)    // If it's the first room
            nextRoom = rooms[rooms.Count - 1];     // Just loop to the end for now.
        else
            nextRoom = rooms[rooms.IndexOf(currentRoom) - 1];
        
        currentRoom = nextRoom;

        currentRoom.gameObject.SetActive(true);                             // Activate the new room.
        rooms[rooms.IndexOf(currentRoom) - 1].gameObject.SetActive(false);  // Deactivate previous room.

        // If the next room is a checkpoint, update the current checkpoint and save the current resources (PlayerPrefs).
        if(currentRoom.IsCheckpoint)
        {
            PlayerPrefs.SetInt("Checkpoint", rooms.IndexOf(currentRoom)); 

            PlayerPrefs.SetInt("Anger", ResourceManager.Instance.Rage);
            PlayerPrefs.SetInt("Fear", ResourceManager.Instance.Fear);
            PlayerPrefs.SetInt("Sadness", ResourceManager.Instance.Sadness);
            PlayerPrefs.SetInt("Joy", ResourceManager.Instance.Joy);
        }

        // Teleport the player into the next room and pan the camera.
        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;
        cameraController.MoveTo(currentRoom.CameraSpawn);

        // Make UI popup displaying "Checkpoint Reached" only if the next room is a checkpoint.
        if(currentRoom.IsCheckpoint)
            PathosUI.instance.CheckpointNotif.GetComponent<Animation>().Play();
            //StartCoroutine(DisplayCheckpointPopup());
    }

    /// <summary>
    /// Gets the closest enemy to the last seen location of the player.
    /// </summary>
    public Enemy GetClosestGuardToPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        Enemy closestGuard = currentRoom.Enemies[0];
        float closestDistance = Mathf.Infinity;

        foreach (Enemy guard in currentRoom.Enemies)
        {
            if(guard == null) // A guard could have been destroyed.
                continue;

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

    /// <summary>
    /// Displays the UI popup that shows if the room is a checkpoint or not.
    /// </summary>
    private IEnumerator DisplayCheckpointPopup()
    {
        Transform instance = Instantiate(roomNamePopup);
        instance.SetParent(PathosUI.instance.transform);
        //instance.GetComponent<TextMeshProUGUI>().SetText(currentRoom.RoomName);

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

    private IEnumerator PausePlayerForSeconds(float duration)
    {
        player.GetComponent<PlayerMovement>().Paused = true;

        yield return new WaitForSeconds(duration);

        player.GetComponent<PlayerMovement>().Paused = false;
    }
}
