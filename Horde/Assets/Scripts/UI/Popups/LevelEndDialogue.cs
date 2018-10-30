using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndDialogue : MonoBehaviour {


    [SerializeField]
    private Button continueButton;

    private UnityEngine.Events.UnityAction continueAction;

    public void init(UnityEngine.Events.UnityAction onContinue)
    {
        continueButton.onClick.AddListener(internalContinue);
        continueAction = onContinue;
    }

    private void internalContinue()
    {
       if(continueAction != null)
        {
            continueAction();
        }
        Destroy(gameObject);
    }
}
