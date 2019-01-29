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
		Player player = visionCone.TryGetPlayer();
		if(player == null) // If the player isn't in vision.
		{
			if(!enemyAttack.IsAttacking) // And if the enemy isn't attacking.
			{
				if(enemy.HasPatrolPath)
					enemy.ChangeState(new Patrol(enemy));
				else
					enemy.ChangeState(new Idle(enemy));
			}
		}
		else
		{
			enemyMovement.MoveTo(player.transform.position, enemy.EnemySettings.AlertMovementSpeed);
			
			if(enemyAttack.IsInAttackRange(player.transform.position))
			{
				if(!enemyAttack.IsAttacking)
					enemy.StartCoroutine(enemyAttack.Attack(player.gameObject));
			}
		}
	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.AlertColor);
		visionCone.ChangeRadius(enemy.EnemySettings.AlertVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.AlertVisionConeViewAngle);
        visionCone.ChangePulseRate(.5f);
	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}
}