﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClassEditUIPanel : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

    [SerializeField]
    private InfoDialogue infoDialoguePrefab;
    [SerializeField]
    private InputField nameInputField;
    [SerializeField]
    private ClassEditorUI classEditorUI;

    [SerializeField]
    private ClassUIPanel classUIPanelPrefab;
    [SerializeField]
    private ClassAreaUIPanel classAreaUIPanel;
    [SerializeField]
    private HeuristicUIAccessor heuristicPanelAccessor;
    [SerializeField]
    private GameObject[] heuristicContainers;

    private List<HeuristicUIPanel> panels = new List<HeuristicUIPanel>();

    private bool currentDropValid;

    // the class currently being edited (0-8)
    public int currentClassSlot = 0;

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
            if (panels.Count == heuristicContainers.Length)
            {
                return false;
            }
            addHeuristic(heuristicPanel);
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
        // make sure they have a name entered
        if (nameInputField.text == "")
        {
            InfoDialogue d = GameObject.Instantiate(infoDialoguePrefab);
            d.Init(null, "Please enter a name for your new class");
        }
        // make sure they have at least one behavior
        else if (panels.Count == 0)
        {
            InfoDialogue d = GameObject.Instantiate(infoDialoguePrefab);
            d.Init(null, "Please add behaviors before saving your class");
        }
        else
        {
            saveCurrentClass(nameInputField.text);
            classEditorUI.ToggleMode();
            ClearArea();
        }
    }

    /// <summary>
    /// This method will actually save the current class and add a ClassUIPanel to the ClassAreaUIPanel
    /// </summary>
    /// <param name="name"></param>
    private void saveCurrentClass(string name)
    {
        ClassUIPanel p = GameObject.Instantiate(classUIPanelPrefab);
        p.Init(name, classAreaUIPanel, this, currentClassSlot + 1);
        classAreaUIPanel.AddClassToView(p, currentClassSlot);

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
    /// removes all current heuristics from the area
    /// </summary>
    public void ClearArea()
    {
        foreach(HeuristicUIPanel p in panels)
        {
            if (p != null)
            {
                Destroy(p.gameObject);
            }
        }
        panels.Clear();
        nameInputField.text = "";
    }

    public void LoadHeuristicsFromList(List<HInterface.HType> Heuristics, string name)
    {
        ClearArea();
        foreach(HInterface.HType h in Heuristics)
        {
            HeuristicUIPanel p = heuristicPanelAccessor.GetHeuristicPanel(h);
            addHeuristic(p.gameObject);
        }
        nameInputField.text = name;
    }

    /// <summary>
    /// Exit and clear the class editor UI 
    /// </summary>
    public void Exit()
    {
        ClearArea();
        classEditorUI.ToggleMode();
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

    private void addHeuristic(GameObject heuristicPanel)
    {
        heuristicPanel.transform.SetParent(heuristicContainers[panels.Count].transform, false);
        panels.Add(heuristicPanel.GetComponent<HeuristicUIPanel>());
        RectTransform rt = heuristicPanel.GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(1, 1);
        rt.anchorMin = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.localPosition = Vector3.zero;
    }
}
