using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInfoMenu : MonoBehaviour {

    public GameObject AbilityInfoButtonCanvas;


    private bool isActive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;

            AbilityInfoButtonCanvas.SetActive(isActive);
        }
	}
}
