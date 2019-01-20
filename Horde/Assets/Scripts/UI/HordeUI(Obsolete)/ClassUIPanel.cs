using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassUIPanel : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private Text classNameText;

    public bool Selected = false;

    // if this is true this class panel opens up the class editor when clicked
    public bool CreateNewHButton = false;
    [SerializeField]
    private ClassEditUIPanel classEditUIPanel;

    private Image panel;
    private Color normalPanelColor;
    private Color selectedPanelColor = new Color(.5f, 1f, .5f);

    public List<HInterface.HType> Heuristics = new List<HInterface.HType>();
    private ClassAreaUIPanel classPanelContainer;

    [SerializeField]
    private Text numberKeyText;

    private float lastClickTime = 0f;

    public void Init(string className, ClassAreaUIPanel container, ClassEditUIPanel classEditUIPanel, int numberKey)
    {
        classNameText.text = className;
        panel = GetComponent<Image>();
        normalPanelColor = panel.color;
        classPanelContainer = container;
        this.classEditUIPanel = classEditUIPanel;
        numberKeyText.text = numberKey.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // check if we are the class UI panel that is supposed to be opening the edit menu
        if (!CreateNewHButton)
        {
            Select(true);
        }
        else
        {
            ClassEditorUI.Instance.ToggleMode();
            classEditUIPanel.currentClassSlot = transform.GetSiblingIndex();
        }
    }

    public void Select(bool allowDoubleClick = false)
    {
        if (CreateNewHButton)
        {
            return;
        }

        Selected = true;
        panel.color = selectedPanelColor;
        classPanelContainer.OnPanelSelected(this);

        if (allowDoubleClick && Time.time - lastClickTime <= 0.5f)
        {
            classEditUIPanel.LoadHeuristicsFromList(Heuristics, classNameText.text);
            classEditUIPanel.currentClassSlot = transform.GetSiblingIndex();
            ClassEditorUI.Instance.ToggleMode();
        }

        lastClickTime = Time.time;
    }

    public void DeSelect()
    {
        Selected = false;
        panel.color = normalPanelColor;
    }
}
