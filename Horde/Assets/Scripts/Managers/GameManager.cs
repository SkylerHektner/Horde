using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private StateMachine stateMachine;

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
        stateMachine = new StateMachine(new InitializePhase());

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
        stateMachine.ExecuteStateUpdate();
	}
}
