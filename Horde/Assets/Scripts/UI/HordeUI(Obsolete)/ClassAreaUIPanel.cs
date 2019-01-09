using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassAreaUIPanel : MonoBehaviour
{
    private List<ClassUIPanel> classUIPanels = new List<ClassUIPanel>();

    [SerializeField]
    private ClassUIPanel openEditorButton;

    
    private void Update()
    {
        // I am ashamed of myself for doing this this way
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.GetChild(0).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.GetChild(1).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.GetChild(2).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.GetChild(3).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.GetChild(4).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.GetChild(5).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            transform.GetChild(6).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            transform.GetChild(7).GetComponent<ClassUIPanel>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            transform.GetChild(8).GetComponent<ClassUIPanel>().Select();
        }
    }
    /// <summary>
    /// Returns the currently selected ClassUIPanel. 
    /// If there is no selected panel, returns null.
    /// </summary>
    /// 
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
    public void AddClassToView(ClassUIPanel ClassUIPanel, int index)
    {
        Destroy(transform.GetChild(index).gameObject);

        ClassUIPanel.transform.SetParent(transform);
        classUIPanels.Add(ClassUIPanel);
        ClassUIPanel.transform.SetSiblingIndex(index);
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
