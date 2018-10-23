using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndDialogue : MonoBehaviour {


    [SerializeField]
    private Button playAgainButton;

    [SerializeField]
    private Button exitLevelButton;

    [SerializeField]
    private Text promptText;

    private UnityEngine.Events.UnityAction playAgainAction;
    private UnityEngine.Events.UnityAction exitLevelAction;

    public void init(UnityEngine.Events.UnityAction onPlayAgain,
        UnityEngine.Events.UnityAction onExitLevel, string promptText,
        string playAgainText = "Play Again", string exitLevelText = "Exit Level")
    {
        playAgainButton.onClick.AddListener(internalPlayAgain);
        if (onExitLevel != null)
        {
            exitLevelAction = onExitLevel;
        }
        exitLevelButton.onClick.AddListener(internalExitLevel);
        playAgainButton.GetComponentInChildren<Text>().text = playAgainText;
        exitLevelButton.GetComponentInChildren<Text>().text = exitLevelText;
        this.promptText.text = promptText;

        playAgainAction = onPlayAgain;
    }

    private void internalExitLevel()
    {
        if(exitLevelAction != null)
        {
            exitLevelAction();
        }
        Destroy(gameObject);
    }

    private void internalPlayAgain()
    {
       if(playAgainAction != null)
        {
            playAgainAction();
        }
        Destroy(gameObject);
    }
}
