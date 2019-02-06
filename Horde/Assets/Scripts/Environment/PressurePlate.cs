using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour {

    public GameObject[] listeners;
    public bool guardsCanTrigger = false;

    private int onCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || guardsCanTrigger)
        {
            onCount++;
            if (listeners != null && onCount == 1)
            {
                foreach (GameObject handler in listeners)
                {
                    handler.GetComponent<ITriggerHandler>().TriggerOn();
                }
                Debug.Log("TRIGGERED");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || guardsCanTrigger)
        {
            onCount--;
            if (listeners != null && onCount == 0)
            {
                foreach (GameObject handler in listeners)
                {
                    handler.GetComponent<ITriggerHandler>().TriggerOff();
                }
                Debug.Log("UN TRIGGERED");
            }
        }
    }
}
