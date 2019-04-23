using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount;

    [SerializeField] private bool IsFirstPickup;
    [SerializeField]
    [Tooltip("The button that will be activated when the item is picked up")]
    //public GameObject TargetButton;
    public ActivateButtonEvent ActivateButton;
    [SerializeField] private bool NonResourcePickup;
    [SerializeField] private string ButtonName;

    public void Start()
    {
        if (ActivateButton == null)
        {
            ActivateButton = new ActivateButtonEvent();
        }

        ActivateButton.AddListener(PathosUI.instance.ActivateButton);
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            if (!NonResourcePickup)
            {
                ResourceManager.Instance.AddEmotion(resourceType, amount);
            }
            if(IsFirstPickup)
            {
                PathosUI.instance.NotificationEvent.Invoke();
                PlayerPrefs.SetInt(ButtonName + "Button", 1);
                InventoryController.instance.InventoryChangedEvent.Invoke();
            }
            Destroy(gameObject);
        }
    }
}
