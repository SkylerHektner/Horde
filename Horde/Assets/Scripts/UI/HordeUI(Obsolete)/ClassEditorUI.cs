using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassEditorUI : MonoBehaviour {

    public static ClassEditorUI Instance;

    [SerializeField]
    private GameObject heuristicsPanel;
    [SerializeField]
    private GameObject editArea;
    [SerializeField]
    private GameObject classPanel;
    [SerializeField]
    private Button toggleButton;

    public ClassAreaUIPanel classAreaUIPanel { get { return classPanel.GetComponentInChildren<ClassAreaUIPanel>(); } }

    private string placeUnitsText = "Place Units";
    private string editClassesText = "Edit Classes";

    public bool InEditMode = true;

    private void Start()
    {
        Instance = this;
        swapButtonText();
    }

    public void ShowBaseUnitPanel()
    {
        heuristicsPanel.SetActive(false);
        editArea.SetActive(false);
    }

    public void HideBaseUnitPanel()
    {
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


    public List<HInterface.HType> GetCurrentSpell()
    {
        return classAreaUIPanel.CurrentSelectedPanel.Heuristics;
    }
}
