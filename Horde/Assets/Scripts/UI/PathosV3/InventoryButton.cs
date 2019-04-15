﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryButtonPressedEvent : UnityEvent<MenuInfo, Button> { }

public class InventoryButton : MonoBehaviour
{
    [Header("INFORMATION")]
    [Tooltip("The Info to show up on the menu when this button is clicked")]
    public MenuInfo Information;
    public string ButtonName;

    [Space(10)]
    public bool Reset;

    private InventoryController inventoryController;
    private InventoryButtonPressedEvent buttonPressedEvent;
    private bool _Active;
    private bool _HasStarted = false;

    // The button that is clicked. This information is sent to the Inventory Controller to hilight the correct active button
    private Button button;

    /*
     * get the inventoryController and set up the event
    */
    void Start()
    {
        if(Reset)
        {
            PlayerPrefs.DeleteKey(ButtonName);
        }
        _HasStarted = true;
        inventoryController = InventoryController.instance;
        if(buttonPressedEvent == null)
        {
            buttonPressedEvent = new InventoryButtonPressedEvent();
        }
        buttonPressedEvent.AddListener(inventoryController.SetUpInformation);
        button = GetComponent<Button>();
        button.onClick.AddListener(SendInfo);
        if(PlayerPrefs.GetInt(ButtonName+ "button") == 1)
        {
            _Active = true;
        }
    }

    private void OnEnable()
    {
        if(!_HasStarted)
        {
            return;
        }
        if(_Active)
        {
            button.enabled = true;
            button.GetComponent<Image>().enabled = true;
            button.GetComponentInChildren<Text>().enabled = true;
            return;
        }
        else if(PlayerPrefs.GetInt(ButtonName+"button") == 0)
        {
            button.enabled = false;
            button.GetComponent<Image>().enabled = false;
            button.GetComponentInChildren<Text>().enabled = false;
        }   
    }

    /*
     * Called when the Button Event is Fired
     */
    public void SendInfo()
    {
        if (button != null && Information != null)
        {
            buttonPressedEvent.Invoke(Information, button);
        }
        else if (Information == null) 
        {
            Debug.Log("NO INFORMATION");
        }
    }
}
