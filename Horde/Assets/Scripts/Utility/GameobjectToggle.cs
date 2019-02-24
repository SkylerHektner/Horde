using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GameobjectToggle : MonoBehaviour
{
    public GameObject[] Parents;
    public ToggleMode toggleMode;

    public enum ToggleMode
    {
        EnableOnEnter,
        DisableOnEnter
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (toggleMode)
            {
                case (ToggleMode.DisableOnEnter):
                    foreach (GameObject g in Parents)
                    {
                        g.SetActive(false);
                    }
                    break;
                case (ToggleMode.EnableOnEnter):
                    foreach (GameObject g in Parents)
                    {
                        g.SetActive(true);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (toggleMode)
            {
                case (ToggleMode.DisableOnEnter):
                    foreach (GameObject g in Parents)
                    {
                        g.SetActive(true);
                    }
                    break;
                case (ToggleMode.EnableOnEnter):
                    foreach (GameObject g in Parents)
                    {
                        g.SetActive(false);
                    }
                    break;
            }
        }
    }
}
