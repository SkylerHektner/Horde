using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	public EnemySettings EnemySettings { get { return enemySettings; } }
	public bool HasPatrolPath { get { return hasPatrolPath; } }

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool hasPatrolPath;
	[SerializeField] private List<Vector3> patrolPoints;

	private NavMeshAgent agent;
	private EnemyAttack enemyAttack;

	private AIState currentState;

	private void Awake() 
	{
		agent = GetComponent<NavMeshAgent>();
		enemyAttack = GetComponent<EnemyAttack>();
		
		// Set to idle or patrol state
		if(hasPatrolPath)
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

	public void ChangeState(AIState state)
	{
		if(currentState != null)
			currentState.LeaveState();

		currentState = state;
	}
}
