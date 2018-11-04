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

    // if this is true this class panel opens up the class editor when clicked
    public bool CreateNewHButton = false;
    [SerializeField]
    private ClassEditorUI classEditorUI;

    private Image panel;
    private Color normalPanelColor;
    private Color selectedPanelColor = new Color(.5f, 1f, .5f);

    public List<HInterface.HType> Heuristics = new List<HInterface.HType>();
    public Unit baseUnitPrefab;
    private ClassAreaUIPanel classPanelContainer;

    private float lastClickTime = 0f;

    public void Init(string className, ClassAreaUIPanel container, ClassEditUIPanel classEditUIPanel, Unit baseUnitPrefab)
    {
        classNameText.text = className;
        panel = GetComponent<Image>();
        normalPanelColor = panel.color;
        classPanelContainer = container;
        this.classEditUIPanel = classEditUIPanel;
        this.baseUnitPrefab = baseUnitPrefab;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // check if we are the class UI panel that is supposed to be opening the edit menu
        if(CreateNewHButton)
        {
            classEditorUI.ToggleMode();
            return;
        }

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
