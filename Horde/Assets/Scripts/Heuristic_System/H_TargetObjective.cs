using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_TargetObjective : Heuristic
{
    private Unit objective;

    public override void Init()
    {
        base.Init(); // Sets 'unit' to the current unit that this heuristic is on.

        // if there are no objectives just return

        if (objective == null)
        {
            objective = UnitManager.instance.GetObjective();
        }
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
        unit.CurrentTarget = objective; // Set the unit's current target to it's weakest enemy.
        yield return new WaitForSeconds(0.1f);
        Resolve();
    }
}

