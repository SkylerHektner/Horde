using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class GetAbilityInfoEvent : UnityEvent<string, string, string> { }

public class AbilityButton : MonoBehaviour {

    [SerializeField]
    [Header("Ability Info")]
    [Tooltip("The Name of the ability")]
    public string AbilityName;
    [Tooltip("How the ability works as well as any Lore about the ability")]
    public string AbilityText;
    [Tooltip("The filename of the ability")]
    public VideoClip AbilityClip;

    [SerializeField]
    [Header("Ability Button Scripts")]
    [Tooltip("The controller that passes the Ability Info to the Ability info panel")]
    public AbilityButtonUIController Controller;
    [Tooltip("The button that contains this ability info")]
    public Button ActualButton;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<Button>().enabled = false;
        gameObject.GetComponentInChildren<Text>().enabled = false;
	}
	
    public void SendInfo()
    {
        Controller.AbilityButtonPressed(AbilityName, AbilityText, AbilityClip, ActualButton);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
