using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassUIPanel : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private Text classNameText;
    

    public bool Selected = false;

    private Image panel;
    private Color normalPanelColor;

    public List<HInterface.HType> Heuristics = new List<HInterface.HType>();
    private ClassAreaUIPanel classPanelContainer;

    public void Init(string className, ClassAreaUIPanel container)
    {
        classNameText.text = className;
        panel = GetComponent<Image>();
        normalPanelColor = panel.color;
        classPanelContainer = container;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected = true;
        panel.color = normalPanelColor * 2;
        classPanelContainer.OnPanelSelected(this);
    }

    public void DeSelect()
    {
        Selected = false;
        panel.color = normalPanelColor;
    }
}
