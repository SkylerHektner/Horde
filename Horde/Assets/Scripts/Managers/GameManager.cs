using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public delegate void CaptureAction();
    public static event CaptureAction OnCaptured; // When the enemy catches the player.

    private Unit[] enemies;

    [SerializeField]
    private Transform[] checkpoints;
    private Transform currentCheckpoint;

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
    }
	
	private void Update ()
    {

	}

    public void ResetLevel()
    {
        Debug.Log("Reached");
        foreach(Unit enemy in enemies)
        {
            Destroy(enemy.GetComponent<Heuristic>()); // Remove whatever heuristic it's executing.
            enemy.transform.position = enemy.GetComponent<Unit>().InitialPosition; // Set position to it's initial location.
            enemy.GetComponent<Unit>().UnitController.ResetPatrolPathing(); // Make sure the unit starts at the beginning of his patrol.

            PlayerManager.instance.Player.transform.position = checkpoints[0].position;
        }
    }
}
