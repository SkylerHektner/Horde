using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	[SerializeField] private EnemySettings enemySettings;

	private NavMeshAgent agent;
	private AIState currentState;

	private void Awake() 
	{
		agent = GetComponent<NavMeshAgent>();
		
		// Set to idle or patrol state
		if(enemySettings.HasPatrolPath)
			ChangeState(new Patrol(this, enemySettings));
		else
			ChangeState(new Idle(this, enemySettings));
	}
	
	private void Update() 
	{
		
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
}
