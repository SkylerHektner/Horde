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

    private InventoryController inventoryController;
    private InventoryButtonPressedEvent buttonPressedEvent;

    // The button that is clicked. This information is sent to the Inventory Controller to hilight the correct active button
    private Button button;

    /*
     * get the inventoryController and set up the event
    */
    void Start()
    {
        inventoryController = InventoryController.instance;
        if(buttonPressedEvent == null)
        {
            buttonPressedEvent = new InventoryButtonPressedEvent();
        }
        buttonPressedEvent.AddListener(inventoryController.SetUpInformation);
        button = GetComponent<Button>();
        button.onClick.AddListener(SendInfo);
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
