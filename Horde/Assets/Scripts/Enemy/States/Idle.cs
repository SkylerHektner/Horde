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
	private float preAlertDuration;

	public Idle(Enemy enemy): base(enemy)
	{
		preAlertDuration = enemy.EnemySettings.PreAlertDuration; 
	}

	public override void Tick()
	{
		Player player = visionCone.TryGetPlayer(); // Null if the player isn't in vision.

		//Debug.Log(enemy.IsDistracted);
		if(!enemy.IsDistracted && player == null) // Don't reset position if currently distracted or if player in vision.
		{
			preAlertDuration = enemy.EnemySettings.PreAlertDuration; // Reset the timer if player isn't in vision.

			if(!AtSpawnPosition()) 
			{
				enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
			}
			else if(AtSpawnPosition() && enemy.transform.rotation != enemy.SpawnRotation)
			{
				ResetRotation();
			}
		}
		else if(player != null) // If the player is within vision.
		{
			preAlertDuration -= Time.smoothDeltaTime; // Count down the pre-alert duration.

			// Stare at the target until the pre-alert duration is over.
			StareAtTarget(player);
			if(preAlertDuration <= 0)
			{
				GameManager.Instance.AlertGuards();
			}
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

	private void StareAtTarget(Player p)
	{
		enemyMovement.LookAt(p.transform.position);
	}

	private void ResetPosition()
	{
		enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
	}

	private void ResetRotation()
	{
		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemy.SpawnRotation, 5.0f * Time.deltaTime);
	}

	private bool AtSpawnPosition()
	{
		// We don't care if y values are different.
		Vector3 enemyPosition = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
		Vector3 spawnPosition = new Vector3(enemy.SpawnPosition.x, 0, enemy.SpawnPosition.z);

		if(Vector3.Distance(enemyPosition, spawnPosition) < 0.25f)
		{
			return true;
		}

		return false;
	}
}
