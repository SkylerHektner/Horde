using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance; // Singleton instance

    public Room CurrentCheckpoint { get { return currentCheckpoint; } set { currentCheckpoint = value; } }

    private Room currentCheckpoint;

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
