using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInfoMenu : MonoBehaviour {

    [SerializeField]
    [Header("UI PANELS")]
    [Tooltip("The panel that the buttons appear on")]
    public GameObject AbilityInfoButtonPanel;
    [Tooltip("The panel that the ability information will appear on")]
    public GameObject AbilityInfoPanel;

    [SerializeField]
    [Header("UI CONTROL SCRIPTS")]
    [Tooltip("The ability info script on the AbilityInfoPanel")]
    public AbilityInfoUI AbilityInfo;
    [Tooltip("The button controller script within the AbilityInfoButtonPanel")]
    public AbilityButtonUIController ButtonController;

    private bool isActive;

	// Use this for initialization
	void Start () {
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
            Debug.Log("Text is: " + description);
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

            if(!isActive)
            {
                AbilityInfoPanel.SetActive(false);
                ButtonController.DisableActiveButton();
            }
            AbilityInfoButtonPanel.SetActive(isActive);
        }
	}
}
