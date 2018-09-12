using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentlyRunningState;
    private IState previousState;

    /// <summary>
    /// Constructor -- Starts the state machine with the state that
    ///                is passed in.
    /// </summary>
    /// <param name="initialState"></param>
    public StateMachine(IState initialState)
    {
        currentlyRunningState = initialState;
        currentlyRunningState.Enter();
    }

    /// <summary>
    /// Changes to the state that is passed in.
    /// The exit funtion of the previous state and the
    /// enter function of the new state is called.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IState newState)
    {
        currentlyRunningState.Exit();

        currentlyRunningState = newState;
        currentlyRunningState.Enter();
    }

    /// <summary>
    /// This should be called on every tick.
    /// Calls the state's execute function.
    /// </summary>
    public void ExecuteStateUpdate()
    {
         currentlyRunningState.Execute();
    }

    /// <summary>
    /// Returns the current state.
    /// </summary>
    /// <returns></returns>
    public IState GetCurrentState()
    {
        return currentlyRunningState;
    }
}
