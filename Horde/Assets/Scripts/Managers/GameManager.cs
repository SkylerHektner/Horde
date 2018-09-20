using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private GameObject allyContainer;

    [SerializeField]
    private Unit baseUnitPrefab;
    [SerializeField]
    private ClassAreaUIPanel classUIAreaPanel;
    [SerializeField]
    private ClassEditorUI classEditorUI;

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
            UnitManager.instance.StartAllyAI();
        }
    }

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        CurGameState = GameState.Setup;
        allyContainer = GameObject.Find("Allies");
    }
	
	private void Update ()
    {
        // check if the player is trying to place units
        if (CurGameState == GameState.Setup && 
            Input.GetMouseButtonDown(0) && 
            !classEditorUI.InEditMode && 
            !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (classUIAreaPanel.CurrentSelectedPanel != null)
                {
                    Unit u = Instantiate(baseUnitPrefab);
                    u.transform.position = hitInfo.point + new Vector3(0, 0.5f);
                    u.behaviors = classUIAreaPanel.CurrentSelectedPanel.Heuristics;
                    u.transform.parent = allyContainer.transform;
                    UnitManager.instance.UpdateUnits();
                }
            }
        }
	}
}
