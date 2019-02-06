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
		if(enemy.SpawnPosition != null)
		{
			enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
		}
		    
	}

	public override void Tick()
	{
		if(enemy.transform.position != enemy.SpawnPosition)
			enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
			
		if(visionCone.TryGetPlayer())
		{
			//EnemyManager.instance.AlertEnemies();
			enemy.ChangeState(new Alert(enemy));
		}
	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.DefaultColor);
		visionCone.ChangeRadius(enemy.EnemySettings.DefaultVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.DefaultVisionConeViewAngle);
        visionCone.ChangePulseRate(0.3f);
	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}
}
