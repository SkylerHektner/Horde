using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivateButtonEvent: UnityEvent<GameObject> { }

public class ActivateUIButton : MonoBehaviour
{
    
    [SerializeField]
    [Tooltip("The button that will be activated when the item is picked up")]
    public GameObject TargetButton;
    public ActivateButtonEvent ActivateButton;

    private Button button;
    private Text buttonText;
    private Image buttonImage;

    public void Start()
    {
        if(ActivateButton == null)
        {
            ActivateButton = new ActivateButtonEvent();
        }

        ActivateButton.AddListener(PathosUI.instance.ActivateButton);
    } 

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("HERE");
            ActivateButton.Invoke(TargetButton);
        }
    }
}
