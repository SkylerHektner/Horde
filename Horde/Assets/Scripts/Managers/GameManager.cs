using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private GameObject allyContainer;

    [SerializeField]
    private int MaxUnitsForLevel = 10;
    [SerializeField]
    private ClassEditorUI classEditorUI;
    private ClassAreaUIPanel classUIAreaPanel;
    [SerializeField]
    private Text unitCapText;

    public GameState CurGameState { get; private set; }

    public enum GameState
    {
        Setup,
        Simulate
    }

    public void StartSimulation()
    {
        if (CurGameState == GameState.Setup)
        {
            CurGameState = GameState.Simulate;
            UnitManager.instance.StartAI();
        }
    }

    private void Start()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        classUIAreaPanel = classEditorUI.classAreaUIPanel;
        CurGameState = GameState.Setup;
        allyContainer = GameObject.Find("TeamOne");
        UpdateUnitCaptionText();
    }
	
	private void Update ()
    {
        // check if the player is trying to place units
        if (CurGameState == GameState.Setup && 
            Input.GetMouseButtonDown(0) && 
            !classEditorUI.InEditMode && 
            UnitManager.instance.TeamOneUnitCount < MaxUnitsForLevel &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if(classUIAreaPanel == null)
                {
                    classUIAreaPanel = classEditorUI.classAreaUIPanel;
                }
                if (classUIAreaPanel.CurrentSelectedPanel != null)
                {
                    //Unit u = Instantiate(classUIAreaPanel.CurrentSelectedPanel.baseUnitPrefab, hitInfo.point + new Vector3(0, 0.5f), Quaternion.identity);
                    //u.behaviors = classUIAreaPanel.CurrentSelectedPanel.Heuristics;
                    //u.transform.parent = allyContainer.transform;
                    //UnitManager.instance.UpdateUnits();
                    //UpdateUnitCaptionText();
                }
            }
        }
	}

    private void UpdateUnitCaptionText()
    {
        unitCapText.text = UnitManager.instance.TeamOneUnitCount.ToString() + "/" + MaxUnitsForLevel.ToString();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
