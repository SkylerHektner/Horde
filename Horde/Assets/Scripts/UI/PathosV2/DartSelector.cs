using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartSelector : MonoBehaviour 
{
	[SerializeField]
	private List<Text> capsuleNumbers;

	private Color startingTextColor;

	private void Start()
	{
		startingTextColor = capsuleNumbers[0].color;

		// By default, rage is selected.
		PathosUI.instance.CurrentEmotion = ResourceManager.ResourceType.Rage;

		ClearSelectedNumberColor();
		capsuleNumbers[0].color = Color.red;
	}

	private void Update()
	{
		if(Input.GetKeyDown("1"))
		{
			PathosUI.instance.CurrentEmotion = ResourceManager.ResourceType.Rage;

			ClearSelectedNumberColor();
			capsuleNumbers[0].color = Color.red;
		}
		else if(Input.GetKeyDown("2"))
		{
			PathosUI.instance.CurrentEmotion = ResourceManager.ResourceType.Joy;

			ClearSelectedNumberColor();
			capsuleNumbers[1].color = Color.red;
		}
		else if(Input.GetKeyDown("3"))
		{
			PathosUI.instance.CurrentEmotion = ResourceManager.ResourceType.Sadness;

			ClearSelectedNumberColor();
			capsuleNumbers[2].color = Color.red;
		}
		else if(Input.GetKeyDown("4"))
		{
			PathosUI.instance.CurrentEmotion = ResourceManager.ResourceType.Fear;

			ClearSelectedNumberColor();
			capsuleNumbers[3].color = Color.red;
		}
	}

	/// <summary>
	/// Sets all the capsule numbers back to their defaul black color.
	/// </summary>
	private void ClearSelectedNumberColor()
	{
		foreach(Text t in capsuleNumbers)
		{
			t.color = startingTextColor;
		}
	}
}
