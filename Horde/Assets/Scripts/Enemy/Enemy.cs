using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	public EnemySettings EnemySettings { get { return enemySettings; } }

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool isPatrolling;
	[SerializeField] private List<Vector3> patrolPoints;

	private NavMeshAgent agent;
	private EnemyAttack enemyAttack;

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
		enemyAttack = GetComponent<EnemyAttack>();
		
		// Set to idle or patrol state
		if(isPatrolling)
			ChangeState(new Patrol(this));
		else
			ChangeState(new Anger(this));
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
}
