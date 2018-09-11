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

    private bool CopyOnDrag = true;

    private Transform lastParent;
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

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

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
