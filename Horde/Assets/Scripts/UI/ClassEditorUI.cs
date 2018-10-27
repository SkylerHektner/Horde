using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassEditorUI : MonoBehaviour {

    [SerializeField]
    private GameObject heuristicsPanel;
    [SerializeField]
    private GameObject editArea;
    [SerializeField]
    private GameObject classPanel;
    [SerializeField]
    private Button toggleButton;

    private string placeUnitsText = "Place Units";
    private string editClassesText = "Edit Classes";

    public bool InEditMode = true;

    [SerializeField]
    private Vector2 classAnchorMinEdit;
    [SerializeField]
    private Vector2 classAnchorMaxEdit;
    [SerializeField]
    private Vector2 classAnchorMinPlay;
    [SerializeField]
    private Vector2 classAnchorMaxPlay;

    private void Start()
    {
        swapButtonText();
    }

    public void ToggleMode()
    {
        RectTransform rt = classPanel.GetComponent<RectTransform>();
        if (!InEditMode)
        {
            heuristicsPanel.SetActive(true);
            editArea.SetActive(true);
            rt.anchorMin = classAnchorMinEdit;
            rt.anchorMax = classAnchorMaxEdit;
            classPanel.GetComponentInChildren<ClassAreaUIPanel>().EnteringEditView();
        }
        else
        {
            heuristicsPanel.SetActive(false);
            editArea.SetActive(false);
            rt.anchorMin = classAnchorMinPlay;
            rt.anchorMax = classAnchorMaxPlay;
            classPanel.GetComponentInChildren<ClassAreaUIPanel>().EnteringPlayView();
        }
        rt.sizeDelta = Vector2.zero;
        InEditMode = !InEditMode;
        swapButtonText();
    }

    private void swapButtonText()
    {
        if (InEditMode)
        {
            toggleButton.GetComponentInChildren<Text>().text = placeUnitsText;
        }
        else
        {
            toggleButton.GetComponentInChildren<Text>().text = editClassesText;
        }
    }
}
