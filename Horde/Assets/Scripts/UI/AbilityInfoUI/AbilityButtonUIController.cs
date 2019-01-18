using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class AbilityButtonEvent : UnityEvent<string,string,string, bool>{}

public class AbilityButtonUIController : MonoBehaviour {

    public AbilityButtonEvent ButtonEvent;
    public Color ActiveButtonColor;

    private ColorBlock cb;
    private Color normalButtonColor;
    private Color highlightedButtonColor;
    private string activeString = "NONE";
    private Button activeButton;

    public void AbilityButtonPressed(string name, string description, string filename, Button sender)
    {
        bool alreadyactive = (activeString == name);
        ButtonEvent.Invoke(name, description, filename, !alreadyactive);

        if(!alreadyactive)
        {
            if(activeString != "NONE")
            {
                cb = activeButton.colors;
                cb.normalColor = normalButtonColor;
                cb.highlightedColor = highlightedButtonColor;
                activeButton.colors = cb;
            }
            activeButton = sender;
            activeString = name;
            normalButtonColor = sender.colors.normalColor;
            highlightedButtonColor = sender.colors.highlightedColor;

            cb = sender.colors;
            cb.normalColor = ActiveButtonColor;
            cb.highlightedColor = ActiveButtonColor;
            sender.colors = cb;
        }
        else
        {
            activeString = "NONE";
            cb = sender.colors;
            cb.normalColor = normalButtonColor;
            cb.highlightedColor = highlightedButtonColor;
            sender.colors = cb;
        }
    }

	// Use this for initialization
	void Awake () {
		if (ButtonEvent == null)
        {
            ButtonEvent = new AbilityButtonEvent();
        }
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
