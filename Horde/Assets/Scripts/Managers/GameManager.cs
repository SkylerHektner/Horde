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
    public Room StartingRoom { set { startingRoom = value;} }

    public bool RoomIsAlerted { get { return roomIsAlerted; } set { roomIsAlerted = value; } }
    public Player Player { get { return player; } }
    public bool PlayerIsMarked { get { return playerIsMarked; } set { playerIsMarked = value; } }
    public float OutOfVisionDuration { get { return outOfVisionDuration; } set { outOfVisionDuration = value; } }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private FadeCamera fadeCamera;
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room startingRoom;
    [SerializeField] private Transform roomNamePopup;

    private Room currentRoom;
    private Room lastCheckpoint;

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
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        currentRoom = startingRoom;
        lastCheckpoint = startingRoom;

        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;

        cameraController.MoveTo(currentRoom.CameraSpawn);
    }

    private void Update()
    {
        OutOfVisionDuration += Time.smoothDeltaTime;

        // Lock the door if guards are alerted.
        if (roomIsAlerted)
            currentRoom.Exit.LockDoor();
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
        currentRoom.gameObject.SetActive(true); // Activate the new room.
        rooms[rooms.IndexOf(currentRoom) - 1].gameObject.SetActive(false);

        player.GetComponent<NavMeshAgent>().Warp(currentRoom.Spawn.position);
        player.transform.rotation = currentRoom.Spawn.rotation;

        cameraController.MoveTo(currentRoom.CameraSpawn);
        StartCoroutine(DisplayRoomName());
    }

    private IEnumerator DisplayRoomName()
    {
        Transform instance = Instantiate(roomNamePopup);
        instance.SetParent(PathosUI.instance.transform);
        instance.GetComponent<TextMeshProUGUI>().SetText(currentRoom.RoomName);

        yield return new WaitForSeconds(3.0f);

        Destroy(instance.gameObject);
    }
}
