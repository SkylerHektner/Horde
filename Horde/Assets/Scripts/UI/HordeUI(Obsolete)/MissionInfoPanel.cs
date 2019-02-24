using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionInfoPanel : MonoBehaviour {
    public static MissionInfoPanel Instance;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Text missionTitle;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button backButton;

    // the level that will be loaded when the user presses play
    private string currentLevel;

    private void Start()
    {
        Instance = this;
    }

    public void ShowUI(string MissionTitle, string MissionDescription, string Level)
    {
        missionTitle.text = MissionTitle;
        descriptionText.text = MissionDescription;
        currentLevel = Level;
        toggleAllUI(true);
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void HideUI()
    {
        toggleAllUI(false);
        //CameraController.Instance.lockPanControls = false;
        //CameraController.Instance.setTargetZoom(30);
    }

    private void toggleAllUI(bool show)
    {
        background.gameObject.SetActive(show);
        missionTitle.gameObject.SetActive(show);
        descriptionText.gameObject.SetActive(show);
        playButton.gameObject.SetActive(show);
        backButton.gameObject.SetActive(show);
    }
}
