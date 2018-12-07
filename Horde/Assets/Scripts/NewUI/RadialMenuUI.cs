using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuUI : MonoBehaviour
{
	public static RadialMenuUI Instance; // Singleton instance

	[SerializeField]
	private GameObject radialMenu;

	[SerializeField]
	private GameObject clearButton;

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

	[SerializeField]
	private CapsuleSelector capsuleSelector;

    public int curBehaviorCount
    {
        get
        {
            return capsuleSelector.SelectedCapsule.ContainedHeuristics.Count;
        }
    }

    public bool InEditMode { get { return radialMenu.activeInHierarchy; } }

	private void Awake () 
	{
		// Make sure only one instance of this class exists. (Singleton)
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
	}

	private void Start()
	{
		radialMenu.SetActive(false);
		clearButton.SetActive(false);
	}

	private void Update()
	{
		if(Input.GetKey("tab"))
		{
			radialMenu.SetActive(true);
			clearButton.SetActive(true);
		}
		else
		{
			radialMenu.SetActive(false);
			clearButton.SetActive(false);
		}
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
		ClearDisplayBar();

		foreach(var v in c.ContainedHeuristics)
		{
			GameObject fillerGO = Instantiate(filler);
			fillerGO.transform.SetParent(displayBar.transform.Find("Layout"));

			fillerGO.transform.Find("Icon").GetComponent<Image>().sprite = v.icon;
			fillerGO.GetComponent<Image>().color = v.color;
		}
	}

	public void ClearDisplayBar()
	{
		foreach(Transform child in displayBar.transform.Find("Layout"))
		{
			Destroy(child.gameObject);
		}
	}

	public void ClearCapsule()
	{
		ClearDisplayBar();
		capsuleSelector.SelectedCapsule.ClearCapsule();
	}

	public List<HInterface.HType> GetHeuristicChain()
	{
		List<HInterface.HType> heuristics = new List<HInterface.HType>();
		Capsule capsule = capsuleSelector.SelectedCapsule;

		foreach(var v in capsule.ContainedHeuristics)
		{
			heuristics.Add(v.heuristic);
		}

		return heuristics;
	}
}
