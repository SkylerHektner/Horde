using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoDialogue : MonoBehaviour {

    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Text promptText;

	public void Init(UnityEngine.Events.UnityAction OnConfirm, 
        UnityEngine.Events.UnityAction OnCancel, string promptText,
        string confirmText = "Accept", string cancelText = "Cancel")
    {
        if (OnConfirm != null)
        {
            confirmButton.onClick.AddListener(OnConfirm);
        }
        confirmButton.onClick.AddListener(internalOnConfirm);
        if (OnCancel != null)
        {
            cancelButton.onClick.AddListener(OnCancel);
        }
        cancelButton.onClick.AddListener(internalOnCancel);
        confirmButton.GetComponentInChildren<Text>().text = confirmText;
        cancelButton.GetComponentInChildren<Text>().text = cancelText;
        this.promptText.text = promptText;
    }

    private void internalOnCancel()
    {
        Destroy(gameObject);
    }

    private void internalOnConfirm()
    {
        Destroy(gameObject);
    }
}
