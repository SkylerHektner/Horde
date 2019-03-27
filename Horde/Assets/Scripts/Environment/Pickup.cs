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
            ResourceManager.Instance.AddEmotion(resourceType, amount);
            /*
            if(IsFirstPickup)
            {
                ActivateButton.Invoke(TargetButton);
            }
            */
            Destroy(gameObject);
        }
    }
}
