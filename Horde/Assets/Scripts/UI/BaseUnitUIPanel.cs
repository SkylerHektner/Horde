using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseUnitUIPanel : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private Unit baseUnitPrefab;
    [SerializeField]
    private ClassEditUIPanel classEditUIPanel;
    [SerializeField]
    private ClassEditorUI classEditorUI;
    [SerializeField]
    private Text baseUnitName;
    [SerializeField]
    private Image baseUnitPortrait;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            //classEditUIPanel.AssignBaseUnit(baseUnitPrefab, baseUnitName.text, baseUnitPortrait.sprite);
            //classEditorUI.HideBaseUnitPanel();
        }
    }
}
