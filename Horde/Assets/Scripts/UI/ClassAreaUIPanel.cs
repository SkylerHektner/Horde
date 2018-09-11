using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassAreaUIPanel : MonoBehaviour
{
    private List<GameObject> classUIPanels = new List<GameObject>();

    public void AddClassToView(GameObject ClassUIPanel)
    {
        ClassUIPanel.transform.SetParent(transform);
        classUIPanels.Add(ClassUIPanel);
        recalculateBounds();
    }

    [ContextMenu("Re-Calculate Bounds")]
    private void recalculateBounds()
    {
        float width = 0f;
        foreach (GameObject p in classUIPanels)
        {
            width += p.GetComponent<RectTransform>().rect.width;
        }
        HorizontalLayoutGroup l = GetComponent<HorizontalLayoutGroup>();
        width += l.spacing * (classUIPanels.Count - 1);
        width += l.padding.horizontal;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
    }
	
}
