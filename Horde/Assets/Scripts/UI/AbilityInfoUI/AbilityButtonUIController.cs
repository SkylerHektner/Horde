using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AbilityButtonEvent : UnityEvent<string,string,string>{}

public class AbilityButtonUIController : MonoBehaviour {

    public AbilityButtonEvent ButtonEvent;

    public void AbilityButtonPressed(string name, string description, string filename)
    {
        ButtonEvent.Invoke(name, description, filename);
    }

	// Use this for initialization
	void Start () {
		if (ButtonEvent == null)
        {
            ButtonEvent = new AbilityButtonEvent();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
