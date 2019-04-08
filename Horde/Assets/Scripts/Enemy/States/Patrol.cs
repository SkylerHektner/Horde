using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// --[ Patrol State ]--
/// Walks along a patrol route.
/// If the player enters the guards vision for a duration, the room will be alerted.
/// </summary>
public class Patrol : AIState
{
	private List<Transform> patrolPointList;	
	private int destPoint = 0; 						// The current destination in the patrol path.
	private float preAlertDuration;					// The amount of time the player can be in vision before Alerting the guards.
	private Player player;


	public Patrol(Enemy enemy): base(enemy) { }

	public override void InitializeState()
	{
		patrolPointList = new List<Transform>();
		preAlertDuration = enemy.EnemySettings.PreAlertDuration;

		SetPatrolPoints();
		MoveToNextPatrolPoint();
	}

	public override void Tick()
	{
		base.Tick();

		player = visionCone.TryGetPlayer();

		// Alert the guards if the player gets too close (even if not inside of the vision cone).
		if(PlayerTooClose())
		{
			enemy.transform.LookAt(GameManager.Instance.Player.transform);
			GameManager.Instance.AlertGuards();
		}

		// REMINDER: A guard is "distracted" while it is staring as a sad guard.
		if(!enemy.IsDistracted && player == null)
		{
			preAlertDuration = enemy.EnemySettings.PreAlertDuration; 	// Reset the timer if player isn't in vision.
			enemyMovement.ResumePath();									// And resume its patrol path.

			// Follow the normal patrol route.
			if(!agent.pathPending && agent.remainingDistance < 0.01f)
			{
				MoveToNextPatrolPoint();
			}
		}
		else if(player != null) // Player is in vision.
		{
			enemyMovement.PausePath();
			StareAtTarget(player);

			preAlertDuration -= Time.smoothDeltaTime;
			if(preAlertDuration <= 0)
			{
				GameManager.Instance.AlertGuards();
			}
		}
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

	private void MoveToNextPatrolPoint()
    {
        enemyMovement.MoveTo(patrolPointList[destPoint].position, enemy.EnemySettings.DefaultMovementSpeed);

        destPoint = (destPoint + 1) % patrolPointList.Count; // Increment the index
    }

	/// <summary>
	/// Patrol Option:
    /// Concatenates the patrol points forwards and then backwards to create a patrolling path.
    /// e.g. given patrol points A, B, C, the patrol path will be updated to A -> B -> C -> C -> B -> A
	///
	/// Loop Option:
	/// Keeps the patrol points in the same order for a looping path.
	/// e.g. given patrol points A, B, C, the patrol path will be updated to A -> B -> C
    /// </summary>
    private void SetPatrolPoints()
    {
		// Set the points to a patrolling path.
		if(enemy.PatrolType == PatrolType.Patrol)
		{
			List<Transform> points = new List<Transform>(enemy.PatrolPoints);
			List<Transform> pointsReversed = new List<Transform>(enemy.PatrolPoints);
			pointsReversed.Reverse();

			List<Transform> mergedList = new List<Transform>();
			mergedList.AddRange(points);
			mergedList.AddRange(pointsReversed);

			patrolPointList = mergedList;
		}
		else if(enemy.PatrolType == PatrolType.Loop)
		{
			patrolPointList = enemy.PatrolPoints;
		}
    }

	/// <summary>
	/// Turns the rotation of the guard towards the direction of the player.
	/// </summary>
	private void StareAtTarget(Player p)
	{
		enemyMovement.LookAt(p.transform.position);
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
}
