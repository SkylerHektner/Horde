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
    [SerializeField]
    private GameObject BaseUnitSelectPanel;
    [SerializeField]
    private GameObject BaseUnitInfoPanel;

    public ClassAreaUIPanel classAreaUIPanel { get { return classPanel.GetComponentInChildren<ClassAreaUIPanel>(); } }

    private string placeUnitsText = "Place Units";
    private string editClassesText = "Edit Classes";

    public bool InEditMode = true;

    private void Start()
    {
        swapButtonText();
    }

    public void ShowBaseUnitPanel()
    {
        BaseUnitSelectPanel.SetActive(true);
        heuristicsPanel.SetActive(false);
        editArea.SetActive(false);
    }

    public void HideBaseUnitPanel()
    {
        BaseUnitSelectPanel.SetActive(false);
        heuristicsPanel.SetActive(true);
        editArea.SetActive(true);
    }

    /// <summary>
    /// Switches between edit mode and play mode
    /// </summary>
    public void ToggleMode()
    {
        if (!InEditMode)
        {
            heuristicsPanel.SetActive(true);
            editArea.SetActive(true);
            classPanel.SetActive(false);
        }
        else
        {
            heuristicsPanel.SetActive(false);
            editArea.SetActive(false);
            classPanel.SetActive(true);
            BaseUnitSelectPanel.SetActive(false);
            BaseUnitInfoPanel.SetActive(false);
        }
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
