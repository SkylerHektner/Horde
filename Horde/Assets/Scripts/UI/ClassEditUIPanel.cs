using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClassEditUIPanel : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

    [SerializeField]
    private TextInputDialogue textInputDialoguePrefab;
    [SerializeField]
    private ClassUIPanel classUIPanelPrefab;
    [SerializeField]
    private ClassAreaUIPanel classAreaUIPanel;

    private List<HeuristicUIPanel> panels = new List<HeuristicUIPanel>();

    private bool currentDropValid;

    /// <summary>
    /// Heuristic Panels call this when they get dropped.
    /// Returns true if the drop was on the class edit area
    /// Also re-assigns the parent if the drop was valid.
    /// </summary>
    /// <param name="heuristicPanel"></param>
    /// <returns></returns>
    public bool HeuristicDropped(GameObject heuristicPanel)
    {
        if (currentDropValid)
        {
            heuristicPanel.transform.SetParent(transform);
            panels.Add(heuristicPanel.GetComponent<HeuristicUIPanel>());
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method will take the current heuristic chain and save
    /// it as a new class
    /// </summary>
    public void SaveCurrentClass()
    {
        TextInputDialogue d = GameObject.Instantiate(textInputDialoguePrefab);
        d.Init(saveCurrentClass, null, "What would you like to name your class?", "Class Name");
    }

    private void saveCurrentClass(string name)
    {
        ClassUIPanel p = GameObject.Instantiate(classUIPanelPrefab);
        p.Init(name);
        classAreaUIPanel.AddClassToView(p.gameObject);

        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] == null)
            {
                panels.RemoveAt(i);
                i--;
            }
            else
            {
                p.Heuristics.Add(panels[i].heuristic);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DraggablesController.Instance.CurrentDraggable != null)
        {
            currentDropValid = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentDropValid = false;
    }
}
