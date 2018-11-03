using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Move --
/// 
/// Moves to the current target. Simple as that.
/// Resolves as soon as the unit reaches the target, or if the target dies before that.
/// </summary>
public class H_Move : Heuristic
{
	private Vector3 targetPosition;

	public override void Init()
	{
		base.Init();

		targetPosition = unit.CurrentTarget.transform.position;	
		unit.UnitController.MoveTo(targetPosition);
	}

	public override void Execute()
	{
		// Resolve if the unit reaches the target or if the target dies.
		if(unit.CurrentTarget == null || DistanceFromTarget() <= 2)
		{
			unit.UnitController.StopMoving();
			Resolve();
		}
	}

	public override void Resolve()
	{
		base.Resolve();
	}

	private float DistanceFromTarget()
	{
		return Vector3.Distance(transform.position, targetPosition);
	}
}
