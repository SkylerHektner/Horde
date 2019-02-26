using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour 
{

    [SerializeField]
    public GameObject Menu;
    public GameObject MenuTexts;
    public GameObject Settings;
    public Transform cameraLocationSettings;
    public Transform cameraLocationMenu;
    public GameObject MovingMan;
    public GameObject place;

    bool moving = false;
    public float timer = 5f;


    private void Start()
    {
        if(Settings != null)
            Settings.SetActive(false);
    }

    public void LoadLevel1()
	{
        moving = true;
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
        //GameManager.Instance.SetCameraLocation(newPos);
        Menu.SetActive(false);
        Settings.SetActive(true);

    }

    public void LeaveSettings()
    {
        Vector3 newPos = new Vector3(cameraLocationMenu.position.x, cameraLocationMenu.position.y, cameraLocationMenu.position.z);
        //.Instance.SetCameraLocation(newPos);
        Menu.SetActive(true);
        Settings.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            timer -= Time.smoothDeltaTime;
            Debug.Log(timer);
            Menu.SetActive(false);
            MenuTexts.SetActive(false);

            MovingMan.transform.GetComponent<Animator>().SetTrigger("MoveAway");
            //Vector3 targetPosition = new Vector3(place.transform.position.x, place.transform.position.y, place.transform.position.z);

            MovingMan.transform.LookAt(place.transform.position);
            MovingMan.transform.Translate(Time.deltaTime, 0, Time.deltaTime * 3);
            if (timer <= 0)
            {
                timer = 0;
                SceneManager.LoadScene("Level1SplitUp");
            }
        }
    }
}
