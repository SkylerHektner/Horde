using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeuristicUIPanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField]
    private Transform rootCanvas;
    [SerializeField]
    private ClassEditUIPanel classEditPanel;
    public HInterface.HType heuristic;

    private bool CopyOnDrag = true;

    private Transform lastParent;

    /// <summary>
    /// When the user begins a drag on the panel we create a copy of ourselves to take our place
    /// and free ourselves from our parent so we can be dragged around.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastParent = transform.parent;
        transform.SetParent(rootCanvas);
        if (CopyOnDrag)
        {
            GameObject replacementPanel = GameObject.Instantiate(gameObject);
            replacementPanel.transform.SetParent(lastParent);
            replacementPanel.gameObject.name = gameObject.name;
            CopyOnDrag = false;
        }
        gameObject.GetComponent<Image>().raycastTarget = false;

        DraggablesController.Instance.CurrentDraggable = gameObject;
    }

    /// <summary>
    /// While the user is dragging us we stay on the cursor
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    /// <summary>
    /// When the user is done dragging us we check with the classEditPanel to see if
    /// we were dropped onto it. 
    /// If we were not, this is now the end of our grand adventure and our life ;-;
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!classEditPanel.HeuristicDropped(gameObject))
        {
            Destroy(gameObject);
        }
        gameObject.GetComponent<Image>().raycastTarget = true;
        DraggablesController.Instance.CurrentDraggable = null;
    }
}
