using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	public EnemySettings EnemySettings { get { return enemySettings; } }
	public bool HasPatrolPath { get { return hasPatrolPath; } }
	public List<Transform> PatrolPoints { get { return patrolPoints; } }
	public Vector3 SpawnPosition { get { return spawnPosition; } }

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool hasPatrolPath;
	[SerializeField] private List<Transform> patrolPoints;

	private NavMeshAgent agent;
	private EnemyAttack enemyAttack;

	private AIState currentState;
	private Vector3 spawnPosition;

	private void Awake() 
	{
		spawnPosition = transform.position;

		agent = GetComponent<NavMeshAgent>();
		enemyAttack = GetComponent<EnemyAttack>();

		// Set to idle or patrol state
		if(hasPatrolPath)
			currentState = new Patrol(this);
		else
			currentState = new Idle(this);
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

	public IEnumerator ChangeStateForDuration(AIState state, float duration)
	{
		if(currentState != null)
			currentState.LeaveState();

		currentState = state;

		yield return new WaitForSeconds(duration);

		currentState.LeaveState();

		if(hasPatrolPath)
			currentState = new Patrol(this);
		else
			currentState = new Idle(this);
	}
}
