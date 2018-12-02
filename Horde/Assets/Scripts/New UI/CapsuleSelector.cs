using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleSelector : MonoBehaviour 
{
	[SerializeField]
	private RadialMenu radialMenu;

	[SerializeField]
	private List<Capsule> capsules;

	[SerializeField]
	private List<Text> capsuleNumbers;

	private Capsule selectedCapsule;
	public Capsule SelectedCapsule { get { return selectedCapsule;} } 

	void Start () 
	{
		selectedCapsule = capsules[0]; // Make the first one the default selected capsule.
		capsuleNumbers[0].color = Color.red;
	}
	
	void Update () 
	{
		if(Input.GetKeyDown("1"))
		{
			ClearSelectedNumberColor();

			capsuleNumbers[0].color = Color.red;
			selectedCapsule = capsules[0];

			radialMenu.UpdateDisplayBar(selectedCapsule);
		}
		else if(Input.GetKeyDown("2"))
		{
			ClearSelectedNumberColor();

			capsuleNumbers[1].color = Color.red;
			selectedCapsule = capsules[1];

			radialMenu.UpdateDisplayBar(selectedCapsule);
		}
		else if(Input.GetKeyDown("3"))
		{
			ClearSelectedNumberColor();

			capsuleNumbers[2].color = Color.red;
			selectedCapsule = capsules[2];

			radialMenu.UpdateDisplayBar(selectedCapsule);
		}
		else if(Input.GetKeyDown("4"))
		{
			ClearSelectedNumberColor();

			capsuleNumbers[3].color = Color.red;
			selectedCapsule = capsules[3];

			radialMenu.UpdateDisplayBar(selectedCapsule);
		}
	}

	public void FillCapsule()
	{

	}

	private void UpdateDisplayBar()
	{

	}

	/// <summary>
	/// Sets all the capsule numbers back to their defaul black color.
	/// </summary>
	private void ClearSelectedNumberColor()
	{
		foreach(Text t in capsuleNumbers)
		{
			t.color = Color.black;
		}
	}
}


