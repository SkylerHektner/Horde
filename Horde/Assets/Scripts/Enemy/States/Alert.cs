using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : AIState
{
	public Alert(Enemy enemy): base(enemy)
	{
		visionCone = enemy.GetComponent<VisionCone>();
		visionCone.ChangeColor(enemy.EnemySettings.AlertColor);
	}

	public override void Tick()
	{
		if(!TryGetPlayer())
		{
			if(enemy.HasPatrolPath)
				enemy.ChangeState(new Patrol(enemy));
			else
				enemy.ChangeState(new Idle(enemy));
		}
		else
		{
			enemyMovement.MoveTo(TryGetPlayer().transform.position);
		}
	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionConeColor()
	{

	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}
}