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
        OpenDoor();
    }

    void Update()
    {
        
    }

    public void UnlockDoor()
    {
        isLocked = false;
        OpenDoor();
        bulb.material.SetColor("_Color", GameManager.Instance.GameSettings.ExitLightColorUnlocked);
        bulb.material.SetColor("_EmissionColor", GameManager.Instance.GameSettings.ExitLightEmissionColorUnlocked);
    }

    public void LockDoor()
    {
        isLocked = true;
        CloseDoor();
        bulb.material.SetColor("_Color", GameManager.Instance.GameSettings.ExitLightColorLocked);
        bulb.material.SetColor("_EmissionColor", GameManager.Instance.GameSettings.ExitLightColorLocked);
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

                //StartCoroutine(closeAfterDelay(2f));
            }
        }
    }

    private IEnumerator closeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseDoor();
    }

    private void OpenDoor()
    {
        GetComponentInChildren<Door>().TriggerOn();
    }

    private void CloseDoor()
    {
        GetComponentInChildren<Door>().TriggerOff();
    }
}
