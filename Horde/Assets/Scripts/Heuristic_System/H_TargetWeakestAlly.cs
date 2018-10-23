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

        // if there are no allies left to seek, just return
        if (gameObject.tag == "TeamOneUnit" && UnitManager.instance.TeamOneUnitCount == 0)
        {
            return;
        }
        else if (gameObject.tag == "TeamTwoUnit" && UnitManager.instance.TeamTwoUnitCount == 0)
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
