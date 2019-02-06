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
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update ()
    {

        if (Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.Mouse0))
        {
            transform.position = Player.GetComponent<Transform>().position;
            Player.gameObject.GetComponent<PlayerMovement>().lockToBack = true;
            Player.gameObject.GetComponent<PlayerMovement>().lockMovementControls = true;
            Player.GetComponent<Animator>().SetBool("Kneeling", true);


        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Player.gameObject.GetComponent<PlayerMovement>().lockToBack = false;
            Player.gameObject.GetComponent<PlayerMovement>().lockMovementControls = false;
            Player.GetComponent<Animator>().SetBool("Kneeling", false);

        }
    }
}
