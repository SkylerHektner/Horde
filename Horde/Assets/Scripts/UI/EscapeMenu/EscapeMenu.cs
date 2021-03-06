﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour {

    public GameObject MenuPanel;

    private bool isActive = false;

    public void Start()
    {
        MenuPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        SceneManager.LoadScene(0);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ActivateEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            MenuPanel.SetActive(isActive);
        }
    }
}
