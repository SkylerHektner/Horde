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

        // if there are no allies left to seek, just return
        if (gameObject.tag == "TeamOneUnit" && UnitManager.instance.TeamOneUnitCount == 0)
        {
            return;
        }
        else if (gameObject.tag == "TeamTwoUnit" && UnitManager.instance.TeamTwoUnitCount == 0)
        {
            return;
        }

        closestAlly = UnitManager.instance.GetClosestAlly(unit);
        StartCoroutine(waitToTarget());
    }

    public override void Execute()
    {
        
    }

    public override void Resolve()
    {
        
        
        base.Resolve(); // Switch to the next heuristic.
    }
    IEnumerator waitToTarget()
    {
        yield return new WaitForSeconds(0.1f);
        unit.CurrentTarget = closestAlly; // Set the unit's current target to it's closest ally.
        Resolve();
    }
}
