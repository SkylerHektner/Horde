using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartSelector : MonoBehaviour 
{
    [SerializeField] private GameObject rageScrim;
    [SerializeField] private GameObject fearScrim;
    [SerializeField] private GameObject sorrowScrim;
	private void Start()
	{
        // By default, rage is selected.
        rageSelected();
	}

	private void Update()
	{
		if(Input.GetKeyDown("1"))
		{
            rageSelected();
		}
		else if(Input.GetKeyDown("2"))
		{
            fearSelected();
        }
		else if(Input.GetKeyDown("3"))
		{
            sadnessSelected();
        }
	}

    private void rageSelected()
    {
        PathosUI.instance.CurrentEmotion = ResourceType.Rage;
        rageScrim.SetActive(true);
        sorrowScrim.SetActive(false);
        fearScrim.SetActive(false);
    }

    private void sadnessSelected()
    {
        PathosUI.instance.CurrentEmotion = ResourceType.Sadness;
        rageScrim.SetActive(false);
        sorrowScrim.SetActive(true);
        fearScrim.SetActive(false);
    }

    private void fearSelected()
    {
        PathosUI.instance.CurrentEmotion = ResourceType.Fear;
        rageScrim.SetActive(false);
        sorrowScrim.SetActive(false);
        fearScrim.SetActive(true);
    }
}
