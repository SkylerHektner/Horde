using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOff : MonoBehaviour
{
    public Light headLamp;
    public AudioSource flashlight;
    public AudioClip ClickOn;
    public AudioClip ClickOff;

    private void Awake()
    {

        headLamp = GetComponent<Light>();
        flashlight = gameObject.GetComponent<AudioSource>();


    }

    private void Update()
    {

        if (headLamp != null && Input.GetKeyUp(KeyCode.L))
        {
            headLamp.enabled = !headLamp.enabled;
            if (headLamp.enabled == true)
            {
                flashlight.clip = ClickOff;
            }
            if (headLamp.enabled == false)
            {
                flashlight.clip = ClickOn;
            }
            flashlight.Play();

        }

    }
}
