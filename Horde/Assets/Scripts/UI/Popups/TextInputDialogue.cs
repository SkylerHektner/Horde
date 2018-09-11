using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputDialogue : MonoBehaviour
{

    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Text promptText;
    [SerializeField]
    private InputField inputField;

    private UnityEngine.Events.UnityAction<string> confirmAction;

    public void Init(UnityEngine.Events.UnityAction<string> OnConfirm,
        UnityEngine.Events.UnityAction OnCancel, string promptText,
        string placeHolderText = "Enter text...",
        string confirmText = "Accept", string cancelText = "Cancel")
    {
        confirmButton.onClick.AddListener(internalOnConfirm);
        if (OnCancel != null)
        {
            cancelButton.onClick.AddListener(OnCancel);
        }
        cancelButton.onClick.AddListener(internalOnCancel);
        confirmButton.GetComponentInChildren<Text>().text = confirmText;
        cancelButton.GetComponentInChildren<Text>().text = cancelText;
        this.promptText.text = promptText;
        inputField.placeholder.GetComponent<Text>().text = placeHolderText;

        confirmAction = OnConfirm;
    }

    private void internalOnCancel()
    {
        Destroy(gameObject);
    }

    private void internalOnConfirm()
    {
        if (confirmAction != null)
        {
            confirmAction(inputField.text);
        }
        Destroy(gameObject);
    }
}
