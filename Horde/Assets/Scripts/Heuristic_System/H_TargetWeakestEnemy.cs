﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Target Weakest Enemy --
/// 
/// Sets the unit's current target to the weakest enemy of the 
/// unit that this heursitic is attached to.
/// </summary>
public class H_TargetWeakestEnemy : Heuristic
{
    private Unit weakestEnemy;

    public override void Init()
    {
        base.Init(); // Sets 'unit' to the current unit that this heuristic is on.

        if (UnitManager.instance.TeamTwoUnitCount == 0) // Check if there are no enemies remaining.
        {
            return;
        }

        weakestEnemy = UnitManager.instance.GetWeakestEnemy(GetComponent<Unit>());
        Resolve();
    }

    public override void Execute()
    {

    }

    public override void Resolve()
    {
        unit.currentTarget = weakestEnemy; // Set the unit's current target to it's weakest enemy.

        base.Resolve(); // Switch to the next heuristic.
    }
}
