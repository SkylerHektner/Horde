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
		if(Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(4);
        }
        else if (Input.GetKey(KeyCode.Alpha5) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(5);
        }
        else if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
