 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Beckon : Heuristic
{
    private float delay = 3f;
    private float curTime = 0f;

	public override void Init()
	{
		base.Init();

		HTargetingTool.Instance.GetTarget(unit, TargetReady, "Select a target to beckon.");
	}

	public override void Execute()
	{
		if(unit.CurrentTarget == null)
            return;

        float distanceFromTarget = Vector3.Distance(transform.position, unit.CurrentTarget.transform.position);

		if(distanceFromTarget <= 3f)
		{
            curTime += Time.deltaTime;
            if(curTime >= delay)
            {
                unit.CurrentTarget.UnitController.StopMoving();
                unit.CurrentTarget.IsMindControlled = false;
                Resolve();
            }
		}
	}

	public override void Resolve()
	{
		base.Resolve();
	}

	private void TargetReady(Unit u, bool success)
	{
        if (!success)
        {
            Resolve();
            return;
        }

        unit.CurrentTarget = u;

		unit.CurrentTarget.IsMindControlled = true;
        Vector3 gap = (u.transform.position - transform.position).normalized * 2f;
        unit.CurrentTarget.UnitController.MoveTo(transform.position + gap);
	}
}
