using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Target Weakest Ally --
/// 
/// Sets the unit's current target to the weakest ally of the 
/// unit that this heursitic is attached to.
/// </summary>
public class H_TargetWeakestAlly : Heuristic
{
    private Unit weakestAlly;

    public override void Init()
    {
        base.Init(); // Sets 'unit' to the current unit that this heuristic is on.

        if (UnitManager.instance.TeamTwoUnitCount == 0) // Check if there are no enemies remaining.
        {
            return;
        }

        weakestAlly = UnitManager.instance.GetWeakestAlly(GetComponent<Unit>());
        Resolve();
    }

    public override void Execute()
    {

    }

    public override void Resolve()
    {
        unit.currentTarget = weakestAlly; // Set the unit's current target to it's weakest ally.

        base.Resolve(); // Switch to the next heuristic.
    }
}
