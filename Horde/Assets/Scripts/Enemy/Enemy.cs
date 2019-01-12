using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	public EnemySettings EnemySettings { get { return enemySettings; } }

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool IsPatrolling;
	[SerializeField] private List<Vector3> patrolPoints;

	private NavMeshAgent agent;
	private AIState currentState;

	private void OnEnable()
	{
		VisionCone.OnPlayerEnteredVision += HandleEnemyEnteredVision;
		VisionCone.OnPlayerExitedVision += HandleEnemyExitedVision;
	}

	private void OnDisable()
	{
		VisionCone.OnPlayerEnteredVision -= HandleEnemyEnteredVision;
		VisionCone.OnPlayerExitedVision -= HandleEnemyExitedVision;
	}

	private void Awake() 
	{
		agent = GetComponent<NavMeshAgent>();
		
		// Set to idle or patrol state
		if(IsPatrolling)
			ChangeState(new Patrol(this));
		else
			ChangeState(new Idle(this));
	}
	
	private void Update() 
	{
		currentState.Tick();
	}

	/// <summary>
	/// Returns the current state of the Enemy.
	/// </summary>
	public AIState GetCurrentState()
	{
		return currentState;
	}

	private void ChangeState(AIState state)
	{
		if(currentState != null)
			currentState.LeaveState();

		currentState = state;
	}

	private void HandleEnemyEnteredVision()
	{
		if(currentState is Idle || currentState is Patrol)
			ChangeState(new Alert(this));
	}

	private void HandleEnemyExitedVision()
	{
		// TEMP
		if(currentState is Alert)
			ChangeState(new Idle(this));
	}

	public override string ToString()
	{
		return currentState != null ? currentState.ToString() : "NONE";
	}
}
