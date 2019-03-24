using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuActivationEvent : UnityEvent<bool> { }

public class PathosUI : MonoBehaviour 
{
	public static PathosUI instance;

	public ResourceType CurrentEmotion { get { return currentEmotion; } set { currentEmotion = value; } }
	private ResourceType currentEmotion;
    public EscapeMenu escMenu;
    public AbilityInfoMenu tabMenu;
    public MenuActivationEvent menuEvent;

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

    public void ActivateButton(GameObject button)
    {
        button.GetComponent<Image>().enabled = true;
        button.GetComponent<Button>().enabled = true;
        button.GetComponentInChildren<Text>().enabled = true;

        string prefstring = button.GetComponent<AbilityButton>().AbilityName;
        PlayerPrefs.SetInt(prefstring, 1);
    }
	
	void Update () 
	{
        if(Input.GetButtonDown("Start"))
        {
            escActive = !escActive;
            if (escActive && tabActive)
            {
                tabActive = false;
                tabMenu.ActivateAbilityMenu();
            }
            menuEvent.Invoke(escActive);
            escMenu.ActivateEscapeMenu();
        }
	    if(Input.GetButtonDown("Help Menu") && !escActive)
        {
            tabActive = !tabActive;
            menuEvent.Invoke(tabActive);
            tabMenu.ActivateAbilityMenu();
        }
	}
}
