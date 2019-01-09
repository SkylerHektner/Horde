using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfDelay : MonoBehaviour {

    public float Delay = 1.0f;
    private float timeSoFar = 0f;
	
	void Update () {
        timeSoFar += Time.deltaTime;
        if (timeSoFar > Delay)
        {
            Destroy(gameObject);
        }
	}
}
