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

        // if there are no enemies left to seek, just return
        if (gameObject.tag == "TeamOneUnit" && UnitManager.instance.TeamTwoUnitCount == 0)
        {
            return;
        }
        else if (gameObject.tag == "TeamTwoUnit" && UnitManager.instance.TeamOneUnitCount == 0)
        {
            return;
        }

        closestEnemy = UnitManager.instance.GetClosestEnemy(GetComponent<Unit>());
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
        unit.CurrentTarget = closestEnemy; // Set the unit's current target to it's closest enemy.
        yield return new WaitForSeconds(0.1f);
        Resolve();
    }
}
