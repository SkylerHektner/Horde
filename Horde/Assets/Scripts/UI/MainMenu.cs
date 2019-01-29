using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	public void LoadLevel1()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadLevelSelect()
	{
		SceneManager.LoadScene("Level Select");
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
