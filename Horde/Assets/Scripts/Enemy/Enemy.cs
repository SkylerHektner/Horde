using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour 
{
	public EnemySettings EnemySettings 	{ get { return enemySettings; } }
	public bool HasPatrolPath 			{ get { return hasPatrolPath; } }
	public List<Transform> PatrolPoints { get { return patrolPoints; } }
	public Vector3 SpawnPosition 		{ get { return spawnPosition; } }
	public Quaternion SpawnRotation	 	{ get { return spawnRotation; } }
	public bool IsDistracted 			{ get { return isDistracted; } set { isDistracted = value; } } // When looking at something. (Like at a crying guard)
	public PatrolType PatrolType 		{ get { return patrolType; } }
    public bool IsDead 					{ get { return currentState.GetType() == typeof(Dead); } }
	public Transform CameraHead			{ get { return cameraHead; } }

	public bool DEBUG_MODE;

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool hasPatrolPath;
	[SerializeField] private PatrolPath patrolPath;
	[SerializeField] private PatrolType patrolType;
	[SerializeField] private Transform cameraHead; 		// Used to rotate the camera head with code.
	[SerializeField] private GameObject recordingIcon; 	// The icon that appears over a guards head when the player is in vision.

    [SerializeField] private GameObject sparkingHeadParticleSystem;

	private NavMeshAgent agent;
	private EnemyAttack enemyAttack;
	private EnemyMovement enemyMovement;
    private Animator enemyAnimator;
    private List<Transform> patrolPoints;
	private AIState currentState;
	private Vector3 spawnPosition;
	private Quaternion spawnRotation;
	private bool isDistracted;
	private float explosionTimer = 0f; // Keeps track of when the enemy should explode.
	private LayerMask enemyMask;
    

    public bool Paused { get; private set; }

	private void Start() 
	{
		SetKinematic(true);

		if(patrolPath != null)
            patrolPoints = patrolPath.GetPatrolPoints();
		
		spawnPosition = transform.position;
		spawnRotation = transform.rotation;

		agent = GetComponent<NavMeshAgent>();
		enemyAttack = GetComponent<EnemyAttack>();
		enemyMovement = GetComponent<EnemyMovement>();
        enemyAnimator = GetComponent<Animator>();

        enemyMask = 1 << LayerMask.NameToLayer("Enemy");

		// Set to idle or patrol state
		if(hasPatrolPath)
			ChangeState(new Patrol(this));
		else
			ChangeState(currentState = new Idle(this));

		currentState.InitializeState();

        PathosUI.instance.menuEvent.AddListener(pause);
	}
	
	private void Update() 
	{
        if (!Paused)
        {
            //Debug.Log(currentState.ToString());
            currentState.Tick();

            // tick down the explosion timer
            explosionTimer -= Time.deltaTime;
            // disable particles if below threshold
            if (explosionTimer < 6f && sparkingHeadParticleSystem != null && sparkingHeadParticleSystem.activeInHierarchy)
            {
                sparkingHeadParticleSystem.SetActive(false);
            }
        }
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
		if(currentState == null)
		{
			currentState = state;
			return;
		}

		// Increase the explosion counter if hit by the same emotion.
		if(currentState.GetType() == state.GetType())
		{
            // increment the explosion timer
            explosionTimer += 6f;

            // enable particle system if greater than thresh
            if (explosionTimer > 6f && sparkingHeadParticleSystem != null)
                sparkingHeadParticleSystem.SetActive(true);

            // explode if greater than thresh
			if(explosionTimer > 12f)
				Explode();
		}
        // reset the explosion timer when a guard gets a new state
		else
		{
            explosionTimer = 6f;
		}
			
        transform.GetComponent<Animator>().SetTrigger("StopTalking");

		currentState.LeaveState();
        currentState = state;
		currentState.InitializeState();
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

	public void ActivateRecIcon()
	{
		recordingIcon.SetActive(true);
	}

	public void DeactivateRecIcon()
	{
		recordingIcon.SetActive(false);
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

    
    private void pause(bool paused)
    {
        Paused = paused;
        enemyAnimator.enabled = !paused;
        agent.isStopped = paused;
    }

    [ContextMenu("Pause")]
    private void testPause()
    {
        pause(true);
    }

    [ContextMenu("UnPause")]
    private void testUnPause()
    {
        pause(false);
    }
}

public enum PatrolType { Patrol, Loop }; 
