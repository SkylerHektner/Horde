using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class AbilityButtonEvent : UnityEvent<string,string,VideoClip, bool>{}

public class AbilityButtonUIController : MonoBehaviour {

    [SerializeField]
    [Header("BUTTON EVENT")]
    [Tooltip("The angle in degrees that the door will change by")]
    public AbilityButtonEvent ButtonEvent;

    [SerializeField]
    [Header("COLOR CONTROL")]
    [Tooltip("The new normal color of the active button")]
    public Color ActiveButtonColor;
    [Tooltip("The new color of the active button when the mouse hovers over it")]
    public Color ActiveButtonHilightColor;

    private ColorBlock cb;
    private Color normalButtonColor;
    private Color highlightedButtonColor;
    private string activeString = "NONE";
    private Button activeButton;

    public void AbilityButtonPressed(string name, string description, VideoClip clip, Button sender)
    {
        bool alreadyactive = (activeString == name);
        ButtonEvent.Invoke(name, description, clip, !alreadyactive);

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
            cb.highlightedColor = ActiveButtonHilightColor;
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

    public void DisableActiveButton()
    {
        if (activeString != "NONE")
        {
            activeString = "NONE";
            cb = activeButton.colors;
            cb.normalColor = normalButtonColor;
            cb.highlightedColor = highlightedButtonColor;
            activeButton.colors = cb;
        }
    }

	// Use this for initialization
	void Awake () {
		if (ButtonEvent == null)
        {
            ButtonEvent = new AbilityButtonEvent();
        }
    }
}
