using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour {

    public GameObject[] listeners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (listeners != null)
            {
                foreach (GameObject handler in listeners)
                {
                    handler.GetComponent<ITriggerHandler>().TriggerOn();
                }
            }
            Debug.Log("TRIGGERED");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (listeners != null)
            {
                foreach (GameObject handler in listeners)
                {
                    handler.GetComponent<ITriggerHandler>().TriggerOff();
                }
            }
            Debug.Log("UN TRIGGERED");
        }

    }
}
