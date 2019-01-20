using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicUIAccessor : MonoBehaviour {


    private List<HeuristicUIPanel> panels = new List<HeuristicUIPanel>();

	private void Start ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            HeuristicUIPanel copyPanel = Instantiate(transform.GetChild(i).gameObject).GetComponent<HeuristicUIPanel>();
            copyPanel.CopyOnDrag = false;
            copyPanel.gameObject.SetActive(false);
            panels.Add(copyPanel);
        }
	}
	
    /// <summary>
    /// Rturns an already configured Heuristic UI Panel for a given
    /// heuristic type. If this type has not yet been configured, returns null
    /// </summary>
    /// <param name="heuristic"></param>
    /// <returns></returns>
	public HeuristicUIPanel GetHeuristicPanel(HInterface.HType heuristic)
    {
        foreach(HeuristicUIPanel p in panels)
        {
            if (p.heuristic == heuristic)
            {
                p.gameObject.SetActive(true);
                return Instantiate(p);
            }
        }

        return null;
    }
}
