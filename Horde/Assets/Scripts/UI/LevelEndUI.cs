using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndUI : MonoBehaviour {

    public UnitManager UnitManager;
    public LevelEndDialogue levelEndDialoguePrefab;

    private string winningTeam = "NO WINNER YET";

	// Use this for initialization
	void Start () {
        UnitManager.LevelEnd.AddListener(displayEndLevel);
	}

    /// <summary>
    /// displays the end level text showing Victory if the TeamOneLose == true else showing failure
    /// </summary>
    /// <param name="TeamOneLose"></param>
    public void displayEndLevel(bool TeamOneLose)
    {
        if(TeamOneLose)
        {
            winningTeam = "Team 2";
        }
        else
        {
            winningTeam = "Team 1";
        }
        LevelEndDialogue LED = GameObject.Instantiate(levelEndDialoguePrefab);
        LED.init(ResetLevel, ExitLevel, winningTeam + " wins! Would you like to play again?");
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitLevel()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
