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
	public Transform ExplosionLocation  { get { return explosionLocation; } }

	public bool DEBUG_MODE;

	[SerializeField] private EnemySettings enemySettings;
	[SerializeField] private bool hasPatrolPath;
	[SerializeField] private PatrolPath patrolPath;
	[SerializeField] private PatrolType patrolType;
	[SerializeField] private Transform cameraHead; 			// Used to rotate the camera head with code.
	[SerializeField] private GameObject recordingIcon; 		// The icon that appears over a guards head when the player is in vision.
	[SerializeField] private Transform explosionLocation; 	// The location where the explosion force comes off the bat.
	[SerializeField] private AudioClip explosionSoundEffect;

    [SerializeField] private GameObject sparkingHeadParticleSystem;
	[SerializeField] private GameObject bloodExplosionParticleEffect;

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
	private LayerMask mask;
	private bool isBarreled;
    

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

        mask = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Breakable");

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
        if (!Paused && !isBarreled)
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

	public void ChangeState(AIState state, bool barreled = false)
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

		
		if(barreled)
		{
			StartCoroutine(UpdateCurrentStateWithDelay(state));	
		}
		else
		{
			currentState.LeaveState();
			currentState = state;
			currentState.InitializeState();
		}
			
	}

	private IEnumerator UpdateCurrentStateWithDelay(AIState state)
	{
		isBarreled = true;
		GetComponent<Animator>().SetTrigger("Barreled");
		GetComponentInChildren<VisionCone>().ChangeRadius(0);
		enemyMovement.Stop();
		currentState.LeaveState();

		yield return new WaitForSeconds(3.0f);

		isBarreled = false;
		
		currentState = state;
		currentState.InitializeState();
	}

	private void Explode()
	{
		GameObject bloodExplosion = Instantiate(bloodExplosionParticleEffect, transform.position, Quaternion.Euler(-90, 0, 0));
		Destroy(bloodExplosion, 3.0f);
		
		// Break nearby breakables or kill nearby guards
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 6.0f, mask);
		foreach(Collider c in objectsInRange)
		{
			if(ReferenceEquals(c.gameObject, gameObject)) // Skip the guard that is exploding.
			{
				continue; 
			} 

			Enemy enemy = c.GetComponent<Enemy>();
			Breakable breakable = c.GetComponent<Breakable>();

			if(enemy)
				enemy.ChangeState(new Dead(enemy));
			else if (breakable)
				breakable.Break();
		}

		// Add an explosive force to all the rigidbodies.
		Collider[] rigidbodies = Physics.OverlapSphere(transform.position, 6.0f);
		foreach(Collider c in rigidbodies)
		{
			Rigidbody rb = c.GetComponent<Rigidbody>();

			if(rb != null)
				rb.AddExplosionForce(50.0f, transform.position, 25.0f, 1.0f, ForceMode.Impulse);
		}

		AudioManager.instance.PlaySoundEffectRandomPitch(explosionSoundEffect);
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
