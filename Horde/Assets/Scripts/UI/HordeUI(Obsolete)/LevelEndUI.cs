using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndUI : MonoBehaviour {

    public UnitManager UnitManager;
    public LevelEndDialogue levelEndDialoguePrefab;

	// Use this for initialization
	void Start () {
        UnitManager.LevelEnd.AddListener(displayEndLevel);
	}

    /// <summary>
    /// displays the end level text showing Victory if the TeamOneLose == true else showing failure
    /// </summary>
    /// <param name="TeamOneLose"></param>
    public void displayEndLevel()
    {
        LevelEndDialogue LED = GameObject.Instantiate(levelEndDialoguePrefab);
        LED.init(ExitLevel);
    }

    private void ExitLevel()
    {
        SerializationManager.Instance.ChangeLevelCompletionStatus(SceneManager.GetActiveScene().name, true);
        SceneManager.LoadScene("LevelSelect");
    }
}
