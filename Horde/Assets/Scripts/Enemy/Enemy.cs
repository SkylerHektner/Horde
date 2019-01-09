using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour 
{
	[SerializeField] private EnemySettings enemySettings;

	private NavMeshAgent agent;
	private EnemyMovement enemyMovement;
	private AIState currentState;

	private void Awake() 
	{
		agent = GetComponent<NavMeshAgent>();
		enemyMovement = new EnemyMovement(enemySettings, agent);
		
		// Set to idle or patrol state
		ChangeState(new Idle(this, enemyMovement));
	}
	
	private void Update() 
	{
		
	}

	public void LookAtTarget(Vector3 pos)
	{
		enemyMovement.RotateTowards(pos);
	}

	private void ChangeState(AIState state)
	{
		if(currentState != null)
			currentState.LeaveState();

		currentState = state;
	}
}
