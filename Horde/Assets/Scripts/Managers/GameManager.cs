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

    private Room[] rooms;
    private GameObject player;

    [SerializeField] private CameraController cameraController;

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
        //rooms = GameObject.Find("Rooms").GetComponentsInChildren<Room>();
    }
	
	private void Update ()
    {

	}

    public void SetCameraLocation(Vector3 v)
    {
        cameraController.SetTargetPos(v);
    }
}
