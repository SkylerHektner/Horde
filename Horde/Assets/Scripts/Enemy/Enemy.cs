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
	public Quaternion SpawnRotation { get { return spawnRotation; } }
	public bool IsDistracted { get { return isDistracted; } set { isDistracted = value; } } // When looking at something. (Like at a crying guard)
	public PatrolType PatrolType { get { return patrolType; } }

    public bool IsDead { get { return currentState.GetType() == typeof(Dead); } }

	public bool DEBUG_MODE;

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool hasPatrolPath;
	[SerializeField] private List<Transform> patrolPoints;
	[SerializeField] private PatrolType patrolType;

	private NavMeshAgent agent;
	private EnemyAttack enemyAttack;
	private EnemyMovement enemyMovement;

	private AIState currentState;
	private Vector3 spawnPosition;
	private Quaternion spawnRotation;
	private bool isDistracted;

	private int explosionCounter; // Keeps track of when the enemy should explode.
	private LayerMask enemyMask;

	private void Start() 
	{
		SetKinematic(true);
		
		spawnPosition = transform.position;
		spawnRotation = transform.rotation;

		agent = GetComponent<NavMeshAgent>();
		enemyAttack = GetComponent<EnemyAttack>();
		enemyMovement = GetComponent<EnemyMovement>();

		enemyMask = 1 << LayerMask.NameToLayer("Enemy");

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
		// Increase the explosion counter if hit by the same emotion.
		if(currentState.GetType() == state.GetType())
		{
			explosionCounter ++;

			if(explosionCounter == 3)
				Explode();
		}
		else
		{
			explosionCounter = 1;
		}
			
        transform.GetComponent<Animator>().SetTrigger("StopTalking");
        currentState = state;
	}

	private void Explode()
	{
		GameObject bloodExplosion = Instantiate(Resources.Load("BloodExplosion2"), transform.position, Quaternion.identity) as GameObject;
		
		// Kill all nearby enemies.
		Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, 6.0f, enemyMask);
		foreach(Collider c in enemiesInRange)
			Destroy(c.gameObject);

		Destroy(gameObject);
	}

	// Used for the ragdoll rigidbodies.
	public void SetKinematic(bool value)
	{
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic = value;
		}
	}

	public void Respawn()
	{
		enemyMovement.Stop();
		enemyMovement.Respawn(spawnPosition);
	}
}

public enum PatrolType { Patrol, Loop }; 
