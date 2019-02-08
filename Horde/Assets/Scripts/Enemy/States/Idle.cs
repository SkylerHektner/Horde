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
		if(!AtSpawnPosition()) 
		{
			enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
		}

		if(AtSpawnPosition() && enemy.transform.rotation != enemy.SpawnRotation)
		{
			Debug.Log("Restting rotation");
			ResetRotation();
		}
			
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
		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemy.SpawnRotation, 20.0f * Time.deltaTime);
	}

	private bool AtSpawnPosition()
	{
		// We don't care if y values are different.
		if(enemy.transform.position.x == enemy.SpawnPosition.x && enemy.transform.position.z == enemy.SpawnPosition.z)
		{
			return true;
		}

		return false;
	}
}
