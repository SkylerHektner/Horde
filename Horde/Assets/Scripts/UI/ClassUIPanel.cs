using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassUIPanel : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private Text classNameText;

    private ClassEditUIPanel classEditUIPanel;

    public bool Selected = false;

    private Image panel;
    private Color normalPanelColor;
    private Color selectedPanelColor = new Color(.5f, 1f, .5f);

    public List<HInterface.HType> Heuristics = new List<HInterface.HType>();
    private ClassAreaUIPanel classPanelContainer;

    private float lastClickTime = 0f;

    public void Init(string className, ClassAreaUIPanel container, ClassEditUIPanel classEditUIPanel)
    {
        classNameText.text = className;
        panel = GetComponent<Image>();
        normalPanelColor = panel.color;
        classPanelContainer = container;
        this.classEditUIPanel = classEditUIPanel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected = true;
        panel.color = selectedPanelColor;
        classPanelContainer.OnPanelSelected(this);

        if (Time.time - lastClickTime <= 0.5f)
        {
            classEditUIPanel.LoadHeuristicsFromList(Heuristics);
        }

        lastClickTime = Time.time;
    }

    public void DeSelect()
    {
        Selected = false;
        panel.color = normalPanelColor;
    }
}
