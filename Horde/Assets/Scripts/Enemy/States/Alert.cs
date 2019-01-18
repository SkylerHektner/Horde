using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	-- Alert State --
/// Chases the player until it loses vision. (All alerted guards share vision)
/// If player is still out of vision, guards play a search animation.(Look around)
/// </summary>
public class Alert : AIState
{
	public Alert(Enemy enemy): base(enemy)
	{
		
	}

	public override void Tick()
	{
		// Just has basic chase behavior right now.

		if(!visionCone.TryGetPlayer()) // If the player isn't in vision.
		{
			if(enemy.HasPatrolPath)
				enemy.ChangeState(new Patrol(enemy));
			else
				enemy.ChangeState(new Idle(enemy));
		}
		else
		{
			enemyMovement.MoveTo(visionCone.TryGetPlayer().transform.position);
		}
	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.AlertColor);
	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}
}