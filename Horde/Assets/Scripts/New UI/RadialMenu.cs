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
	
	private Transform displayBarLayout;

	void Start () 
	{
		displayBarLayout = displayBar.transform.Find("HorizontalLayout");
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

	public void SelectHeuristic()
	{
		GameObject fillerGO = Instantiate(filler);
		fillerGO.transform.SetParent(displayBar.transform.Find("Layout"));

		fillerGO.transform.Find("Icon").GetComponent<Image>().sprite = currentDisplayIcon.sprite;
		fillerGO.GetComponent<Image>().color = centerDisplay.GetComponent<Image>().color;
	}
}
