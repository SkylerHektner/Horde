using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	--[ Idle State ]--
/// Guard doesn't move.
/// If player enters vision for a duration, change to Alert state and alert the other guards.
/// </summary>
public class Idle : AIState
{
	private float preAlertDuration;		// The amount of time the player can be in vision before Alerting the guards.
	private bool canBeStartled;			

	public Idle(Enemy enemy): base(enemy) { }

	public override void InitializeState()
	{
		base.InitializeState();

		preAlertDuration = enemy.EnemySettings.PreAlertDuration; 
	}

	public override void Tick()
	{
		base.Tick();

		Player player = visionCone.TryGetPlayer();

		// Alert the guards if the player gets too close (even if not inside of the vision cone).
		if(PlayerTooClose())
		{
			enemy.transform.LookAt(GameManager.Instance.Player.transform);
			GameManager.Instance.AlertGuards();
		}
			

		// REMINDER: A guard is "distracted" while it is staring as a sad guard.
		if(!enemy.IsDistracted && player == null)
		{
			enemy.GetComponent<Animator>().SetBool("Startled", false);
			canBeStartled = true;
			preAlertDuration = enemy.EnemySettings.PreAlertDuration; 	// Reset the timer if player isn't in vision.
			ResetTransform();											// And reset back to inital position and rotation.
		}
		else if(player != null) // Player is in vision.
		{
			if(canBeStartled)
				enemy.GetComponent<Animator>().SetBool("Startled", true);

			canBeStartled = false;

			StareAtTarget(player);

			preAlertDuration -= Time.smoothDeltaTime;
			if(preAlertDuration <= 0)
			{
				GameManager.Instance.AlertGuards();
                enemy.GetComponent<Animator>().SetBool("Startled", false);
            }
        }
	}

	public override void LeaveState()
	{
		base.LeaveState();

		enemy.GetComponent<Animator>().SetBool("Startled", false);
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

	/// <summary>
	/// Turns the rotation of the guard towards the direction of the player.
	/// </summary>
	private void StareAtTarget(Player p)
	{
		enemyMovement.LookAt(p.transform.position);
	}

	/// <summary>
	/// Checks if the guard is at its initial spawn location of the room.
	/// </summary>
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

	/// <summary>
	/// Checks if the player gets "too close" to the guard.
	/// Used for when the player gets so close to a guard he almosts bumps into him.
	/// The guard needs to know the player is touching him, even though he is not
	/// inside of the vision cone.
	/// </summary>
	private bool PlayerTooClose()
	{
		// Just uses a hard-coded float for distance calculations right now. Should probably change.
		if(Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) < 2.5f) 
			return true;

		return false;
	}

	/// <summary>
	/// Resets the guard back to its initial position and rotation.
	/// Resets the position first, and then resets the rotation.
	/// Also resets the rotation of the head if it's not the correct rotation from spinning.
	/// </summary>
	private void ResetTransform()
	{
		if(!AtSpawnPosition()) 
		{
			enemyMovement.MoveTo(enemy.SpawnPosition, enemy.EnemySettings.DefaultMovementSpeed);
		}
		else if(AtSpawnPosition() && enemy.transform.rotation != enemy.SpawnRotation)
		{
			enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemy.SpawnRotation, 5.0f * Time.deltaTime);
		}
	}
}
