using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{

    public GameObject floorGroup;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            floorGroup.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            floorGroup.SetActive(true);
        }
    }
}
