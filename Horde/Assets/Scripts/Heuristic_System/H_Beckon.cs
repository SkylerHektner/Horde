using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Beckon : Heuristic
{
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

		if(distanceFromTarget <= 1.2f)
		{
			unit.CurrentTarget.UnitController.StopMoving();
			unit.CurrentTarget.IsMindControlled = false;
			Resolve();
		}
			
	}

	public override void Resolve()
	{
		base.Resolve();
	}

	private void TargetReady(Unit u)
	{
		unit.CurrentTarget = u;

		unit.CurrentTarget.IsMindControlled = true;
		unit.CurrentTarget.UnitController.MoveTo(transform.position);
	}
}
