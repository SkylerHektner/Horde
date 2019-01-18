using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GetAbilityInfoEvent : UnityEvent<string, string, string> { }

public class AbilityButton : MonoBehaviour {

    public string AbilityName;
    public string AbilityText;
    public string AbilityImage;
    public AbilityButtonUIController Controller;
    public Button ActualButton;

	// Use this for initialization
	void Start () {
	}
	
    public void SendInfo()
    {
        Controller.AbilityButtonPressed(AbilityName, AbilityText, AbilityImage, ActualButton);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
