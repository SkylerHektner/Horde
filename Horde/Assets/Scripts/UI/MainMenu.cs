using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour 
{

    [SerializeField]
    public GameObject Menu;
    public GameObject Settings;
    public Transform cameraLocationSettings;
    public Transform cameraLocationMenu;


    private void Start()
    {
        Settings.SetActive(false);
    }

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

    public void LoadSettings()
    {
        Vector3 newPos = new Vector3(cameraLocationSettings.position.x, cameraLocationSettings.position.y - 5f, cameraLocationSettings.position.z + 35f);
        GameManager.Instance.SetCameraLocation(newPos);
        Menu.SetActive(false);
        Settings.SetActive(true);

    }

    public void LeaveSettings()
    {
        Vector3 newPos = new Vector3(cameraLocationMenu.position.x, cameraLocationMenu.position.y, cameraLocationMenu.position.z);
        GameManager.Instance.SetCameraLocation(newPos);
        Menu.SetActive(true);
        Settings.SetActive(false);
    }
}
