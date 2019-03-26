using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	--[ Idle State ]--
/// Guard doesn't move.
/// If player enters vision for a duration, change 
/// to Alert state and alert the other guards.
/// </summary>
public class Idle : AIState
{
	private float preAlertDuration;		// The amount of time the player can be in vision before Alerting the guards.

	public Idle(Enemy enemy): base(enemy)
	{
		preAlertDuration = enemy.EnemySettings.PreAlertDuration; 
	}

	public override void Tick()
	{
		Player player = visionCone.TryGetPlayer();

		// Alert the guards if the player gets too close (even if not inside of the vision cone).
		if(PlayerTooClose())
			GameManager.Instance.AlertGuards();

		// REMINDER: A guard is "distracted" while it is staring as a sad guard.
		if(!enemy.IsDistracted && player == null)
		{
			preAlertDuration = enemy.EnemySettings.PreAlertDuration; 	// Reset the timer if player isn't in vision.
			ResetTransform();											// And reset back to inital transform and position.
		}
		else if(player != null) // Player is in vision.
		{
			// Stare at the target until the pre-alert duration is over. Alert the guards if pre-alert duration ends.
			preAlertDuration -= Time.smoothDeltaTime;
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

	private bool PlayerTooClose()
	{
		// Just uses a hard-coded float for distance calculations right now. Should probably change.
		if(Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) < 2.5f) 
			return true;

		return false;
	}

	/// <summary>
	/// Resets the guard back to its initial position and rotation.
	/// </summary>
	private void ResetTransform()
	{
		if(!AtSpawnPosition()) 
		{
			enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
		}
		else if(AtSpawnPosition() && enemy.transform.rotation != enemy.SpawnRotation)
		{
			ResetRotation();
		}
	}
}
