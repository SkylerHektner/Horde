using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Scanner : MonoBehaviour
{

    public GameObject[] listeners;
    float scanTimer = 300f;
    public bool scanning = false;

    private int onCount = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            onCount++;
            scanTimer--;
            if (scanTimer <= 0)
            {
                scanTimer = 0;
                if (listeners != null && onCount == 1)
                {
                    foreach (GameObject handler in listeners)
                    {
                        handler.GetComponent<ITriggerHandler>().TriggerOn();
                    }
                    Debug.Log("Guard TRIGGERED");
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            onCount--;
            if (listeners != null && onCount == 0)
            {
                foreach (GameObject handler in listeners)
                {
                    handler.GetComponent<ITriggerHandler>().TriggerOff();
                }
                Debug.Log("UN Guard TRIGGERED");
                scanTimer = 300f;
            }
        }
    }
}
