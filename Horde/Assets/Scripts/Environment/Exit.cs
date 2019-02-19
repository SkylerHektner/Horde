using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private MeshRenderer bulb;

    private bool isLocked;

    void Start()
    {
        isLocked = false; // The room starts with the door unlocked.
    }

    void Update()
    {
        
    }

    public void UnlockDoor()
    {
        isLocked = false;
        bulb.material.SetColor("_Color", Color.green);
        bulb.material.SetColor("_EmissionColor", Color.green);
    }

    public void LockDoor()
    {
        isLocked = true;
        bulb.material.SetColor("_Color", Color.red);
        bulb.material.SetColor("_EmissionColor", Color.red);
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            if(!isLocked)
            {
                OpenDoor();

                // TODO: Transition animation into the next room.
                GameManager.Instance.TransitionToNextRoom();
            }
        }
    }

    private void OpenDoor()
    {
        if(!isLocked)
            GetComponentInChildren<Door>().TriggerOn();
    }
}
