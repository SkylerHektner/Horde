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
	private float preAlertDuration = 2.0f; // How long it takes until the guard enters the alert state after seeing the player.
	private Vector3 spawnLocation;
	private Quaternion spawnRotation;

	public Idle(Enemy enemy): base(enemy)
	{
		spawnLocation = enemy.Spawn.transform.position;
		spawnRotation = enemy.Spawn.transform.rotation;

		// Go back to original location.
		if(enemy.Spawn != null)
		{
			enemyMovement.MoveTo(spawnLocation, enemy.EnemySettings.DefaultMovementSpeed);
		}
		    
	}

	public override void Tick()
	{
		if(enemy.transform.position != spawnLocation)
			enemyMovement.MoveTo(spawnLocation, enemy.EnemySettings.DefaultMovementSpeed);
		
		if(enemy.transform.rotation != spawnRotation)
			ResetRotation();
			
		if(visionCone.TryGetPlayer())
		{
			preAlertDuration -= Time.smoothDeltaTime;

			StareAtTarget();

			if(preAlertDuration <= 0)
			{
				/*
				foreach(Enemy e in GameManager.Instance.CurrentRoom.Enemies)
				{
					e.ChangeState(new Alert(enemy));
				}
				*/

				enemy.ChangeState(new Alert(enemy));
			}
		}
		else
		{
			preAlertDuration = 2.0f; // Reset the timer if player leaves vision.
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

	private void StareAtTarget()
	{
		Vector3 direction = visionCone.TryGetPlayer().transform.position - enemy.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, desiredRotation, 20.0f * Time.deltaTime);
	}

	private void ResetRotation()
	{
		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, spawnRotation, 20.0f * Time.deltaTime);
	}
}
