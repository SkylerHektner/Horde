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
    /// This method will raise a prompt asking for a name for the new class
    /// </summary>
    public void SaveCurrentClass()
    {
        TextInputDialogue d = GameObject.Instantiate(textInputDialoguePrefab);
        d.Init(saveCurrentClass, null, "What would you like to name your class?", "Class Name");
    }

    /// <summary>
    /// This method will actually save the current class and add a ClassUIPanel to the ClassAreaUIPanel
    /// </summary>
    /// <param name="name"></param>
    private void saveCurrentClass(string name)
    {
        ClassUIPanel p = GameObject.Instantiate(classUIPanelPrefab);
        p.Init(name, classAreaUIPanel);
        classAreaUIPanel.AddClassToView(p);

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

    /// <summary>
    /// If the pointer enters the edit area and it's currently dragging a heuristic, it 
    /// will be a valid drop if the user drops that heuristic
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DraggablesController.Instance.CurrentDraggable != null)
        {
            currentDropValid = true;
        }
    }

    /// <summary>
    /// if the pointer has left the area for any reason then a drop would not be valid
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        currentDropValid = false;
    }
}
