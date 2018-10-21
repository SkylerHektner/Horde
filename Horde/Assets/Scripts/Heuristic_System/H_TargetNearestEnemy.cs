using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Target Nearest Enemy --
/// 
/// Sets the unit's current target to the nearest enemy of the 
/// unit that this heursitic is attached to.
/// </summary>
public class H_TargetNearestEnemy : Heuristic
{
    // Should Target have a range?

    private Unit closestEnemy;
    
    public override void Init()
    {
        base.Init(); // Sets 'unit' to the current unit that this heuristic is on.

        if (UnitManager.instance.TeamTwoUnitCount == 0) // Check if there are no enemies remaining.
        {
            return;
        }

        closestEnemy = UnitManager.instance.GetClosestEnemy(GetComponent<Unit>());
        Resolve();
    }

    public override void Execute()
    {
        // Can't think of anything that needs to be updated every tick.
    }

    public override void Resolve()
    {
        unit.currentTarget = closestEnemy; // Set the unit's current target to it's closest enemy.

        base.Resolve(); // Switch to the next heuristic.
    }
}
