using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEditorUI : MonoBehaviour {

    [SerializeField]
    private GameObject heuristicsPanel;
    [SerializeField]
    private GameObject editArea;
    [SerializeField]
    private GameObject classPanel;

    public bool InEditMode = true;

    [SerializeField]
    private Vector2 classAnchorMinEdit;
    [SerializeField]
    private Vector2 classAnchorMaxEdit;
    [SerializeField]
    private Vector2 classAnchorMinPlay;
    [SerializeField]
    private Vector2 classAnchorMaxPlay;

    public void ToggleMode()
    {
        RectTransform rt = classPanel.GetComponent<RectTransform>();
        if (!InEditMode)
        {
            heuristicsPanel.SetActive(true);
            editArea.SetActive(true);
            rt.anchorMin = classAnchorMinEdit;
            rt.anchorMax = classAnchorMaxEdit;
        }
        else
        {
            heuristicsPanel.SetActive(false);
            editArea.SetActive(false);
            rt.anchorMin = classAnchorMinPlay;
            rt.anchorMax = classAnchorMaxPlay;
        }
        rt.sizeDelta = Vector2.zero;
        InEditMode = !InEditMode;
    }
}
