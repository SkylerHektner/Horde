using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour 
{
	[SerializeField]
	private GameObject menu;

	void Start () 
	{
		menu.SetActive(false);
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(menu.activeSelf == false)
				menu.SetActive(true);
			else if (menu.activeSelf == true)
				menu.SetActive(false);
		}
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
