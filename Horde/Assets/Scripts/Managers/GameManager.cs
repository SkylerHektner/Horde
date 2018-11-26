using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public delegate void CaptureAction();
    public static event CaptureAction OnCaptured; // When the enemy catches the player.

    private Unit[] enemies;
    private GameObject player;

    [SerializeField]
    //private Transform[] checkpoints;
    private Checkpoint[] checkpoints;
    private Checkpoint currentCheckpoint;

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        // Populate the enemies array
        enemies = GameObject.Find("Enemies").GetComponentsInChildren<Unit>();
        player = PlayerManager.instance.Player;

        currentCheckpoint = checkpoints[0];
    }
	
	private void Update ()
    {

	}

    public void ResetLevel()
    {
        player.GetComponent<NavMeshAgent>().Warp(currentCheckpoint.transform.position); // Telepport the player to the current checkpoint.

        enemies = GameObject.Find("Enemies").GetComponentsInChildren<Unit>();
        
        foreach(Unit enemy in enemies)
        {
            Destroy(enemy.GetComponent<Heuristic>()); // Remove whatever heuristic it's executing.
            enemy.GetComponent<NavMeshAgent>().Warp(enemy.GetComponent<Unit>().InitialPosition); // Set position to it's initial location.
            enemy.GetComponent<Unit>().UnitController.ResetPatrolPathing(); // Make sure the unit starts at the beginning of his patrol. 
        }
    }

    /// <summary>
    /// Sets the current checkpoints to the checkpoint that is passed in.
    /// </summary>
    public void SetCheckpoint(Checkpoint cp)
    {
        currentCheckpoint = cp;
    }
}
