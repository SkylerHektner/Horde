using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIState
{
	private List<Transform> patrolPointList;
	private int destPoint = 0; // The current destination in the patrol path.
	private float preAlertDuration;

	public Patrol(Enemy enemy): base(enemy)
	{
		patrolPointList = new List<Transform>();
		preAlertDuration = enemy.EnemySettings.PreAlertDuration;

		SetPatrolPoints();
		MoveToNextPatrolPoint();
	}

	public override void Tick()
	{
		if(enemy.IsDistracted)
		{
			if(enemy.DEBUG_MODE)
				Debug.Log("1");
			enemyMovement.PausePath();
		}
		else if(!enemy.IsDistracted && !visionCone.TryGetPlayer())
		{
			if(enemy.DEBUG_MODE)
				Debug.Log("2");
			preAlertDuration = enemy.EnemySettings.PreAlertDuration; // Reset the timer if player isn't in vision.

			enemyMovement.ResumePath();
			if(!agent.pathPending && agent.remainingDistance < 0.01f)
			{
				MoveToNextPatrolPoint();
			}
		}
		else if(visionCone.TryGetPlayer()) // If the player is within vision and the guards haven't been alerted yet.
		{
			if(enemy.DEBUG_MODE)
				Debug.Log("3");
			preAlertDuration -= Time.smoothDeltaTime; // Count down the pre-alert duration.

			enemyMovement.PausePath();

			// Stare at the target until the pre-alert duration is over.
			StareAtTarget();
			if(preAlertDuration <= 0)
			{
				GameManager.Instance.AlertGuards();
			}
		}
		else if(!agent.pathPending && agent.remainingDistance < 0.01f)
		{
			if(enemy.DEBUG_MODE)
				Debug.Log("4");
			MoveToNextPatrolPoint();
		} 
		else
		{
			if(enemy.DEBUG_MODE)
				Debug.Log("5");
			enemyMovement.ResumePath();
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

	private void StareAtTarget()
	{
		Vector3 direction = visionCone.TryGetPlayer().transform.position - enemy.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, desiredRotation, 5.0f * Time.deltaTime);
	}

	private bool AtSpawnPosition()
	{
		// We don't care if y values are different.
		if(Vector3.Distance(enemy.transform.position, enemy.SpawnPosition) < 0.25f)
		{
			return true;
		}

		return false;
	}

	private void ResetRotation()
	{
		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemy.SpawnRotation, 5.0f * Time.deltaTime);
	}
}
