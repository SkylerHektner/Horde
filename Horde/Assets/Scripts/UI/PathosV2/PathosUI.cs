using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
		if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
	}
	
	void Update () 
	{
        if(Input.GetKeyDown(KeyCode.Escape))
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
	    if(Input.GetKeyDown(KeyCode.Tab) && !escActive)
        {
            tabActive = !tabActive;
            menuEvent.Invoke(tabActive);
            tabMenu.ActivateAbilityMenu();
        }
	}
}
