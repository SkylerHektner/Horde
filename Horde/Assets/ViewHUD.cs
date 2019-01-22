using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHUD : MonoBehaviour
{
    public GameObject Player;
    private GameObject CurrentCam;

    public void Awake()
    {
        transform.position = Player.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update ()
    {

        if (Input.GetKey(KeyCode.R))
        {
            transform.position = Player.GetComponent<Transform>().position;
            Player.gameObject.GetComponent<PlayerMovement>().lockToBack = true;
            Player.gameObject.GetComponent<PlayerMovement>().lockMovementControls = true;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Player.gameObject.GetComponent<PlayerMovement>().lockToBack = false;
            Player.gameObject.GetComponent<PlayerMovement>().lockMovementControls = false;

        }
    }
}
