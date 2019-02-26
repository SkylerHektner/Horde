using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            gameObject.GetComponent<Animator>().SetBool("Loving", true);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            gameObject.GetComponent<Animator>().SetBool("Loving", false);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            gameObject.GetComponent<Animator>().SetBool("Waving", true);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            gameObject.GetComponent<Animator>().SetBool("Waving", false);
        }
    }
}
