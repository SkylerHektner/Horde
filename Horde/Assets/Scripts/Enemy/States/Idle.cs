using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
	public Idle(Enemy enemy): base(enemy)
	{
		// Go back to original location.
	}

	public override void Tick()
	{
		if(TryGetPlayer())
		{
			enemy.ChangeState(new Alert(enemy));
		}
	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionConeColor()
	{
		visionCone.ChangeColor(enemy.EnemySettings.DefaultColor);
	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}
}
