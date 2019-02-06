using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sitdown : MonoBehaviour
{

    public GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }

    }
}
