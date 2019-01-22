using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIState
{
	private List<Transform> patrolPointList;
	private int destPoint = 0; // The current destination in the patrol path.

	public Patrol(Enemy enemy): base(enemy)
	{
		patrolPointList = new List<Transform>();

		SetPatrolPoints();
		MoveToNextPatrolPoint();
	}

	public override void Tick()
	{
		if(visionCone.TryGetPlayer())
		{
			//EnemyManager.instance.AlertEnemies();
			enemy.ChangeState(new Alert(enemy));
		}

		//Debug.Log(agent.path.);
		if(!agent.pathPending && agent.remainingDistance < 0.01f)
		{
			MoveToNextPatrolPoint();
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
        enemyMovement.MoveTo(patrolPointList[destPoint].position);
		//Debug.Log(patrolPointList[destPoint].position);

        destPoint = (destPoint + 1) % patrolPointList.Count; // Increment the index
    }

	/// <summary>
    /// Puts the patrol points in the order that the unit should traverse them in.
    /// e.g. given patrol points A, B, C, this function change the list to A, B, C, C, B, A
    /// </summary>
    private void SetPatrolPoints()
    {
        List<Transform> points = new List<Transform>(enemy.PatrolPoints);
        List<Transform> pointsReversed = new List<Transform>(enemy.PatrolPoints);
        pointsReversed.Reverse();

        List<Transform> mergedList = new List<Transform>();
        mergedList.AddRange(points);
        mergedList.AddRange(pointsReversed);

        patrolPointList = mergedList;

		foreach(Transform t in patrolPointList)
		{
			//Debug.Log(t.position);
		}
    }
}
