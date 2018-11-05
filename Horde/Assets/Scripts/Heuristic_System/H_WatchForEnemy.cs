using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_WatchForEnemy : Heuristic
{
    public override void Init()
    {
        base.Init();
    }

    public override void Execute()
    {
        List<Unit> unitsInRange = UnitManager.instance.GetUnitsInRange(unit);

        if(unitsInRange.Count >= 1)
        {
            unit.CurrentTarget = unitsInRange[0];
            Resolve();
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }
}
