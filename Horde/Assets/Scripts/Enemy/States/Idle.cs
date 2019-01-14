using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	-- Idle State --
/// Enemy doesn't move.
/// If player enters vision, change to Alert state
/// and alert the other guards.
/// </summary>
public class Idle : AIState
{
	public Idle(Enemy enemy): base(enemy)
	{
		// Go back to original location.
	}

	public override void Tick()
	{
		if(visionCone.TryGetPlayer())
		{
			//EnemyManager.instance.AlertEnemies();
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
