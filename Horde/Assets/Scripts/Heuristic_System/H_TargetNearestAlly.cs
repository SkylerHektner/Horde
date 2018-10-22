using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Target Nearest Ally --
/// 
/// Sets the unit's current target to the nearest ally of the 
/// unit that this heursitic is attached to.
/// </summary>
public class H_TargetNearestAlly : Heuristic
{
    private Unit closestAlly;

    public override void Init()
    {
        base.Init(); // Sets 'unit' to the current unit that this heuristic is on.

        if (UnitManager.instance.TeamTwoUnitCount == 0) // Check if there are no enemies remaining.
        {
            return;
        }

        closestAlly = UnitManager.instance.GetClosestAlly(GetComponent<Unit>());
        Resolve();
    }

    public override void Execute()
    {
        
    }

    public override void Resolve()
    {
        unit.currentTarget = closestAlly; // Set the unit's current target to it's closest ally.

        base.Resolve(); // Switch to the next heuristic.
    }
}
