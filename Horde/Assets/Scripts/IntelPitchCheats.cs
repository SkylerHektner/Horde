using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntelPitchCheats : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(Input.GetKeyDown("1"))
        {

        }
        else if(Input.GetKeyDown("2"))
        {

        }
        else if (Input.GetKeyDown("3"))
        {

        }
        else if (Input.GetKeyDown("4"))
        {

        }
        else if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
