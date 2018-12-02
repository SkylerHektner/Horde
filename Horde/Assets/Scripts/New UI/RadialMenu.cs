using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject centerDisplay;

	[SerializeField]
	private Image currentDisplayIcon;

	[SerializeField]
	private Text currentHeuristicName;

	[SerializeField]
	private GameObject displayBar;

	[SerializeField]
	private GameObject filler;

	void Start () 
	{

	}
	
	void Update () 
	{
		
	}

	public void SetCenterIcon(Sprite s)
	{
		currentDisplayIcon.sprite = s;
	}

	public void SetCenterName(string s)
	{
		currentHeuristicName.text = s;
	}

	public void SetCenterColor(Color c)
	{
		centerDisplay.GetComponent<Image>().color = c;
	}

	public void UpdateDisplayBar(Capsule c)
	{
		// Clear it first
		foreach(Transform child in displayBar.transform.Find("Layout"))
		{
			Destroy(child.gameObject);
		}

		foreach(var v in c.ContainedHeuristics)
		{
			GameObject fillerGO = Instantiate(filler);
			fillerGO.transform.SetParent(displayBar.transform.Find("Layout"));

			fillerGO.transform.Find("Icon").GetComponent<Image>().sprite = v.icon;
			fillerGO.GetComponent<Image>().color = v.color;
		}
	}
}
