using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartSelector : MonoBehaviour 
{
    [SerializeField] private GameObject rageScrim;
    [SerializeField] private GameObject fearScrim;
    [SerializeField] private GameObject sorrowScrim;

    private int curSelectionIndex = 0;
    private List<ResourceType> selectionCycle = new List<ResourceType>() { ResourceType.Rage, ResourceType.Fear, ResourceType.Sadness };

	private void Start()
	{
        // By default, rage is selected.
        rageSelected();
	}

	private void Update()
	{
		if(Input.GetButtonDown("Select Rage"))
		{
            rageSelected();
		}
		else if(Input.GetButtonDown("Select Fear"))
		{
            fearSelected();
        }
		else if(Input.GetButtonDown("Select Sorrow"))
		{
            sadnessSelected();
        }

        float mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (mouseScroll != 0f)
        {
            cycleSelection(Mathf.RoundToInt(-mouseScroll));
        }
        else if (Input.GetButtonDown("CycleEmotionPositive"))
        {
            cycleSelection(1);
        }
        else if (Input.GetButtonDown("CycleEmotionNegative"))
        {
            cycleSelection(-1);
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

    private void cycleSelection(int direction)
    {
        curSelectionIndex += direction;
        if (curSelectionIndex < 0)
        {
            curSelectionIndex = selectionCycle.Count - 1;
        }
        if (curSelectionIndex >= selectionCycle.Count)
        {
            curSelectionIndex = 0;
        }
        switch (selectionCycle[curSelectionIndex])
        {
            case ResourceType.Rage:
                rageSelected();
                break;
            case ResourceType.Fear:
                fearSelected();
                break;
            case ResourceType.Sadness:
                sadnessSelected();
                break;
        }
    }
}
