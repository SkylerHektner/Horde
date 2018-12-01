using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject centerDisplay;

	[SerializeField]
	private Image centerDisplayIcon;

	[SerializeField]
	private Text centerHeuristicName;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

	public void SetCenterIcon(Sprite s)
	{
		centerDisplayIcon.sprite = s;
	}

	public void SetCenterName(string s)
	{
		centerHeuristicName.text = s;
	}

	public void SetCenterColor(Color c)
	{
		centerDisplay.GetComponent<Image>().color = c;
	}
}
