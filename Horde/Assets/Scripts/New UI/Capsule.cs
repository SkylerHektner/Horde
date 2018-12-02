using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capsule : MonoBehaviour 
{
	[SerializeField]
	private RadialMenu radialMenu;

	private List<ContainedHeuristic> containedHeuristics;
	public List<ContainedHeuristic> ContainedHeuristics { get { return containedHeuristics; } }
	
	void Start () 
	{
		containedHeuristics = new List<ContainedHeuristic>();
	}
	
	void Update () 
	{
		
	}

	public void AddHeuristicToCapsule(Sprite s, Color c, HInterface.HType h)
	{
		ContainedHeuristic containedHeuristic = new ContainedHeuristic(s, c, h);
		containedHeuristics.Add(containedHeuristic);

		radialMenu.UpdateDisplayBar(this);
	}

	public struct ContainedHeuristic
	{
		public Sprite icon;
		public Color color;
		public HInterface.HType heuristic;

		public ContainedHeuristic(Sprite s, Color c, HInterface.HType h)
		{
			icon = s;
			color = c;
			heuristic = h;
		}
	}
}


