using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePhase : IState
{
    public void Enter()
    {
        // This gets called when the initialize phase starts.
        // Maybe call a function to set up the UI??
    }

    public void Execute()
    {
        // Anything the game needs to execute once per tick during
        // the initial phase should be called here.
    }

    public void Exit()
    {
        // This gets called when the initializing phase is over.
    }
}
