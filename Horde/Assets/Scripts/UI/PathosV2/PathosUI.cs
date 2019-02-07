using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathosUI : MonoBehaviour 
{
	public static PathosUI instance;

	public ResourceManager.ResourceType CurrentEmotion { get { return currentEmotion; } set { currentEmotion = value; } }
	private ResourceManager.ResourceType currentEmotion;
    public EscapeMenu escMenu;
    public AbilityInfoMenu tabMenu;

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
            escMenu.ActivateEscapeMenu();
        }
	    if(Input.GetKeyDown(KeyCode.Tab) && !escActive)
        {
            tabActive = !tabActive;
            tabMenu.ActivateAbilityMenu();
        }
	}
}
