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
        ButtonController.ButtonEvent.AddListener(activateInfoPanel);
	}

    private void activateInfoPanel(string name, string description, string filename)
    {
        AbilityInfoPanel.SetActive(true);
        AbilityInfo.NameText = name;
        AbilityInfo.DescriptionText = description;
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
