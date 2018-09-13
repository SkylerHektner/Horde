using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private StateMachine stateMachine;

    [SerializeField]
    private Unit baseUnitPrefab;
    [SerializeField]
    private ClassAreaUIPanel classUIAreaPanel;
    [SerializeField]
    private ClassEditorUI classEditorUI;

    void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

	void Start ()
    {
        // Initialize a state machine.
        //stateMachine = new StateMachine(new InitializePhase());

        // To switch to the simulation phase, call ChangeState(Istate), like this:
        //
        //     stateMachine.ChangeState(new SimulationPhase());
        //
        // This will call the exit function of the previous state and the enter
        // function of the new state.
	}
	
	void Update ()
    {
        // Execute whichever state we are in on every tick.
        //stateMachine.ExecuteStateUpdate();
        if (Input.GetMouseButtonDown(0) && !classEditorUI.InEditMode)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (classUIAreaPanel.CurrentSelectedPanel != null)
                {
                    Unit u = Instantiate(baseUnitPrefab);
                    u.transform.position = hitInfo.point + new Vector3(0, 0.5f);
                    u.behaviors = classUIAreaPanel.CurrentSelectedPanel.Heuristics;
                }
            }
        }
	}
}
