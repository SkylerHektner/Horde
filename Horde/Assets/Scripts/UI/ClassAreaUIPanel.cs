using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassAreaUIPanel : MonoBehaviour
{
    private List<ClassUIPanel> classUIPanels = new List<ClassUIPanel>();

    [SerializeField]
    private ClassUIPanel openEditorButton;

    /// <summary>
    /// Returns the currently selected ClassUIPanel. 
    /// If there is no selected panel, returns null.
    /// </summary>
    public ClassUIPanel CurrentSelectedPanel
    {
        get
        {
            ClassUIPanel returnVal = null;
            foreach (ClassUIPanel p in classUIPanels)
            {
                if (p.Selected)
                {
                    returnVal = p;
                }
            }
            return returnVal;
        }
    }

    /// <summary>
    /// This is called by the UI edit area when adding a new ClassUIPanel to the class view
    /// </summary>
    /// <param name="ClassUIPanel"></param>
    public void AddClassToView(ClassUIPanel ClassUIPanel)
    {
        ClassUIPanel.transform.SetParent(transform);
        classUIPanels.Add(ClassUIPanel);
        openEditorButton.transform.SetAsLastSibling();
        recalculateBounds();
    }

    /// <summary>
    /// This is used to re calculate the bounds of the scroll container so that our
    /// scroll area works correctly.
    /// </summary>
    [ContextMenu("Re-Calculate Bounds")]
    private void recalculateBounds()
    {
        float width = 0f;
        foreach (ClassUIPanel p in classUIPanels)
        {
            width += p.GetComponent<RectTransform>().rect.width;
        }
        HorizontalLayoutGroup l = GetComponent<HorizontalLayoutGroup>();
        width += l.spacing * (classUIPanels.Count - 1);
        width += l.padding.horizontal;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
    }

    /// <summary>
    /// Panels are responsible for notifying the classAreaController when one of them is selected
    /// so it can tell all the others to deselect
    /// </summary>
    public void OnPanelSelected(ClassUIPanel sender)
    {
        foreach(ClassUIPanel p in classUIPanels)
        {
            if (p.Selected && p != sender)
            {
                p.DeSelect();
            }
        }
    }
	
}
