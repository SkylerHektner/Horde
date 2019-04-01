using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AbilityInfoMenu : MonoBehaviour {

    [SerializeField]
    [Header("UI PANELS")]
    [Tooltip("The panel that the buttons appear on")]
    public GameObject AbilityInfoButtonPanel;
    [Tooltip("The panel that the ability information will appear on")]
    public GameObject AbilityInfoPanel;
    [Tooltip("The video streamer for the info panel")]
    public VideoStreamer streamer;

    [SerializeField]
    [Header("UI CONTROL SCRIPTS")]
    [Tooltip("The ability info script on the AbilityInfoPanel")]
    public AbilityInfoUI AbilityInfo;
    [Tooltip("The button controller script within the AbilityInfoButtonPanel")]
    public AbilityButtonUIController ButtonController;

    private bool isActive;

	// Use this for initialization
	void Start () {
        //ButtonController.ButtonEvent.AddListener(activateInfoPanel);
        AbilityInfoButtonPanel.SetActive(false);
        AbilityInfoPanel.SetActive(false);
	}

    private void activateInfoPanel(string name, string description, VideoClip videoclip, bool activate)
    {
        if (activate)
        { 
            AbilityInfoPanel.SetActive(true);
            AbilityInfo.NameText.text = name;
            Debug.Log("Text is: " + description);
            AbilityInfo.DescriptionText.text = description;
            if(streamer != null)
            {
                streamer.endPlaying();
                streamer.SetVideo(videoclip);
                Debug.Log("CALL PLAY FUNCTION");
                streamer.beginPlaying();
            }
        }
        else
        {
            streamer.endPlaying();
            AbilityInfoPanel.SetActive(false);
        }
    }

    public void ActivateAbilityMenu()
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
