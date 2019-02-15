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
	private Transform currentTarget;
	private float currentTargetBuffer = 2.0f; // The amount of time the current target can be out of vision before losing it.
	private float outOfVisionDuration; // The amount of time the current target has been out of vision.

	public Alert(Enemy enemy): base(enemy)
	{
		
	}

	public override void Tick()
	{
		Player player = visionCone.TryGetPlayer();

		if(player != null)
		{
			currentTarget = player.transform; // Mark the player as the current target.
			outOfVisionDuration = 0; // Reset the outOfVisionDuration counter if the player is in vision.
		}

		if(outOfVisionDuration >= currentTargetBuffer) // If target has been out of vision for extended amount of time.
		{
			currentTarget = null;
		}
			
		if(player == null) // If the player isn't in vision.
		{
			outOfVisionDuration += Time.smoothDeltaTime; // Keep track of the amount of time the target is out of vision.

			if(currentTarget == null)
			{
				if(!enemyAttack.IsAttacking) // And if the enemy isn't attacking.
				{
					if(enemy.HasPatrolPath)
						enemy.ChangeState(new Patrol(enemy));
					else
						enemy.ChangeState(new Idle(enemy));
				}
			}
			
		}
		else
		{
			enemyMovement.MoveTo(currentTarget.transform.position, enemy.EnemySettings.AlertMovementSpeed);
			
			if(enemyAttack.IsInAttackRange(currentTarget.transform.position))
			{
				if(!enemyAttack.IsAttacking)
					enemy.StartCoroutine(enemyAttack.Attack(currentTarget.gameObject));
			}
		}
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