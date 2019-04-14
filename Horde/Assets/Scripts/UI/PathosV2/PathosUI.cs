using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuActivationEvent : UnityEvent<bool> { }

public class PathosUI : MonoBehaviour 
{
    [SerializeField] private DebugWindow debugWindow;
    [SerializeField] private GameObject checkpointNotif;

	public static PathosUI instance;

    public GameObject CheckpointNotif { get { return checkpointNotif; } }
	public ResourceType CurrentEmotion { get { return currentEmotion; } set { currentEmotion = value; } }
	private ResourceType currentEmotion;
    // public EscapeMenu escMenu;
    // public AbilityInfoMenu tabMenu;
    public GameObject PauseMenu;
    public MenuActivationEvent menuEvent;

    public UnityEvent NotificationEvent;

    private bool escActive;
    private bool tabActive;

	private void Awake() 
	{
        if(menuEvent == null)
        {
            menuEvent = new MenuActivationEvent();
        }
		if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
	}

    private void Start()
    {
        debugWindow.gameObject.SetActive(false);
        PauseMenu.SetActive(false);
        if(NotificationEvent == null)
        {
            NotificationEvent = new UnityEvent();
        }
    }

    public void ActivateButton(GameObject button)
    {
        button.GetComponent<Image>().enabled = true;
        button.GetComponent<Button>().enabled = true;
        button.GetComponentInChildren<Text>().enabled = true;

        string prefstring = button.GetComponent<AbilityButton>().AbilityName;
        PlayerPrefs.SetInt(prefstring, 1);
    }
	
    public void DeactivatePauseMenu()
    {
        PauseMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSettings()
    {
        Debug.Log("THERE IS NO CONNECTION TO SETTINGS YET, PLEASE IMPLEMENT BEFORE RELEASE!!!!!");
    }

	void Update () 
	{
        if(Input.GetButtonDown("Start"))
        {
            escActive = !escActive;
            menuEvent.Invoke(escActive);
            PauseMenu.SetActive(true);
        }

	    if(Input.GetButtonDown("Help Menu") && !escActive)
        {
            tabActive = !tabActive;
            menuEvent.Invoke(tabActive);
            if(tabActive)
            {
                InventoryController.instance.ActivateInventory();
            }
            else
            {
                InventoryController.instance.DeactivateInventory();
            }
        }

        // Code to toggle the debug window.
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(debugWindow.gameObject.activeSelf)
            {   
                debugWindow.gameObject.SetActive(false);
                GameManager.Instance.Player.GetComponent<DartGun>().debugWindowOpen = false;
            }
            else
            {
                debugWindow.gameObject.SetActive(true);
                GameManager.Instance.Player.GetComponent<DartGun>().debugWindowOpen = true;
            } 
        }
	}
}
