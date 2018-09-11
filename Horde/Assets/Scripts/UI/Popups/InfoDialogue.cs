using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDialogue : MonoBehaviour
{
    [SerializeField]
    private Button acceptButton;

    [SerializeField]
    private Text promptText;

    public void Init(UnityEngine.Events.UnityAction OnConfirm,
        string promptText, string confirmText = "Accept")
    {
        if (OnConfirm != null)
        {
            acceptButton.onClick.AddListener(OnConfirm);
        }
        acceptButton.onClick.AddListener(internalOnAccept);
        acceptButton.GetComponentInChildren<Text>().text = confirmText;
        this.promptText.text = promptText;
    }

    private void internalOnAccept()
    {
        Destroy(gameObject);
    }
}
