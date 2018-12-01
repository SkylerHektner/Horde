using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implement this interface in your class to allow it to interface with pressure plates and other interactables.
/// </summary>
public interface ITriggerHandler
{
    void TriggerOn();

    void TriggerOff();
}
