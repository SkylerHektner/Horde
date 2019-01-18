using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInfoMenu : MonoBehaviour {

    public GameObject AbilityInfoButtonPanel;
    public GameObject AbilityInfoPanel;
    public AbilityInfoUI AbilityInfo;
    public AbilityButtonUIController ButtonController;

    private bool isActive;

	// Use this for initialization
	void Start () {
        //ButtonController.ButtonEvent.AddListener(activateInfoPanel);
        ButtonController.ButtonEvent.AddListener(activateInfoPanel);
        AbilityInfoButtonPanel.SetActive(false);
        AbilityInfoPanel.SetActive(false);
	}

    private void activateInfoPanel(string name, string description, string filename, bool activate)
    {
        if (activate)
        { 
            AbilityInfoPanel.SetActive(true);
            AbilityInfo.NameText.text = name;
            AbilityInfo.DescriptionText.text = description;
        }
        else
        {
            AbilityInfoPanel.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;

            AbilityInfoButtonPanel.SetActive(isActive);
        }
	}
}
