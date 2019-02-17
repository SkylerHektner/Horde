using System.Collections;
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

    private Room[] rooms;
    private Room currentRoom;

    // Helpers to give guards shared vision during alert state.
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
        rooms = FindObjectsOfType<Room>();
        currentRoom = rooms[0];
    }
	
	private void Update ()
    {
        OutOfVisionDuration += Time.smoothDeltaTime;
	}

    /// <summary>
    /// Alerts all the guards in the room.
    /// </summary>
    public void AlertGuards()
    {
        Debug.Log(currentRoom.Enemies.Count);
        foreach(Enemy e in currentRoom.Enemies)
        {
            if(e.GetCurrentState() is Idle || e.GetCurrentState() is Patrol)
                e.ChangeState(new Alert(e));
        }
    }

    public void SetCameraLocation(Vector3 v)
    {
        cameraController.SetTargetPos(v);
    }
}
