using System.Collections;
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
    public bool AlwaysViewable;

    private InventoryController inventoryController;
    private InventoryButtonPressedEvent buttonPressedEvent;
    private bool _Active;
    private bool _HasStarted = false;

    // The button that is clicked. This information is sent to the Inventory Controller to hilight the correct active button
    private Button button;
    private Text text;
    private Image img;

    /*
     * get the inventoryController and set up the event
    */
    void Start()
    {
        _HasStarted = true;
        inventoryController = InventoryController.instance;
        if(buttonPressedEvent == null)
        {
            buttonPressedEvent = new InventoryButtonPressedEvent();
        }
        buttonPressedEvent.AddListener(inventoryController.SetUpInformation);
        button = GetComponent<Button>();
        button.onClick.AddListener(SendInfo);
        text = GetComponentInChildren<Text>();
        img = GetComponent<Image>();

        if (!AlwaysViewable)
        {
            inventoryController.InventoryChangedEvent.AddListener(CheckActive);
            CheckActive();
        }
    }

    public void CheckActive()
    {
        if(PlayerPrefs.GetInt(ButtonName+"Button") == 1)
        {
            button.enabled = true;
            text.enabled = true;
            img.enabled = true;
        }
        else
        {
            button.enabled = false;
            text.enabled = false;
            img.enabled = false;
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
