﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class represents the Controller in the MVC of the unit.
/// The logic for most of the actions of a unit are in here.
/// </summary>
public class UnitController : MonoBehaviour 
{
    private Unit u;
    private NavMeshAgent agent;

    private bool isPaused = false;
    
    private Transform[] patrolPointList;
    private int destPoint = 0; // The current destination in the patrol path.

    private GameObject player;
    private DrawDetectionRadius detectionRadius;
    
    private void OnEnable()
    {
        HTargetingTool.OnTargeting += Pause;
        HTargetingTool.OnFinishedTargeting += Resume;
        //GameManager.OnCaptured += Reset;
    }

    private void OnDisable()
    {
        HTargetingTool.OnTargeting -= Pause;
        HTargetingTool.OnFinishedTargeting -= Resume;
        //GameManager.OnCaptured -= Reset;
    }

    private void Start()
    {
        u = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();

        detectionRadius = GetComponent<DrawDetectionRadius>();
        detectionRadius.Initialize();

        player = PlayerManager.instance.Player;

        if(u.PatrolPoints != null)
            SetPatrolPoints(); 
    }

    private void Update()
    {
        // If the unit is within capture range of the player, reset the level.
        float distanceFromPlayer = Vector3.Distance(PlayerManager.instance.Player.transform.position, transform.position);
        if(distanceFromPlayer < 3.0f && !u.IsMindControlled) // Random number for now.
        {
            agent.velocity = Vector3.zero; // Make sure the unit stops fully so it doesn't still have momentum when reset to it's initial location.
            agent.ResetPath();
            //GameManager.Instance.ResetLevel();
        }
            
        // We don't want normal behavior to execute if the player is being mind controlled or if the unit is paused.
        if(u.IsMindControlled || isPaused)
            return;

        // If the player isn't mind controlled, execute normal behavior:
        //      * If it's a patrol unit, follow the patrol path.
        //      * If the player enter's the unit's vision, chase it.
        //      * If the unit runs outside of the unit's vision, go back to patrol path.
        //      * If it was a static unit, then go back to initial location.
        //
        if(PlayerInDetectionRange()) // The player entered the detection radius of this unit.
        {
            // Change the color of the detection radius and move to the player.
            detectionRadius.SetToDetectioncolor();
            MoveTo(player.transform.position, 2);
            return;
        }

        detectionRadius.SetToDefaultColor();

        if(u.IsPatrolling)
        {
            if(!agent.pathPending && agent.remainingDistance < 0.01f)
                MoveToNextPatrolPoint();
        } 
        else
        {
            MoveTo(u.InitialPosition); // Static unit should move back to it's initial location.
        }   
    }

    public void MoveToNextPatrolPoint()
    {
        if(patrolPointList.Length == 0) // Return if there aren't any patrol points.
            return;

        MoveTo(patrolPointList[destPoint].position);

        destPoint = (destPoint + 1) % patrolPointList.Length; // Increment the index
    }

    public void Attack()
    {
        //attack.ExecuteAttack(u);

        // TEMP ATTACK FOR NOW
        Vector3 dirVector = (u.CurrentTarget.transform.position - transform.position).normalized;

        GameObject projectileGO;
        projectileGO = Instantiate(Resources.Load("Arrow"), u.ProjectileSpawn.transform.position, Quaternion.identity) as GameObject;

        Projectile p = projectileGO.GetComponent<Projectile>();
        // Set the damage of the projectile.
        p.damage = u.AttackDamage;

        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        //Debug.Log(globalVelocity);
        instance.velocity = dirVector * 25;

        Destroy(instance.gameObject, 3);
    }

    /// <summary>
    /// Commands the NavMesh agent to move to the given target.
    /// </summary>
    public void MoveTo(Vector3 target, int speedMultiplier = 1)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.speed = u.MovementSpeed * speedMultiplier;
    }

    /// <summary>
    /// Removes the unit's current path and stops it's movement.
    /// <summary>
    public void StopMoving()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    /// <summary>
    /// Invokes the special ability of the unit.
    /// </summary>
    public void Special()
    {

    }

    /// <summary>
    /// Subtracts health from the unit. Calls destroy is health drops to
    /// zero or below. Returns true if this instance of damage killed the unit
    /// </summary>
    public bool TakeDamage(int dmgAmount)
    {
        u.CurrentHealth -= dmgAmount;
        // TODO: Call a destroy function if health drops below 0.
        if (u.CurrentHealth <= 0)
        {
            gameObject.transform.SetParent(null); // this is important for UnitManager.UpdateUnits();
            Destroy(gameObject);
            //UnitManager.instance.UpdateUnits();
        }

        return u.CurrentHealth <= 0;
    }

    public bool HealDamage(int dmgAmount)
    {
        if (u.CurrentHealth <= u.MaxHealth)
            u.CurrentHealth += dmgAmount;
 
        return u.CurrentHealth == u.MaxHealth;
    }

    public void ResetPatrolPathing()
    {
        destPoint = 0;
    }

    private bool PlayerInDetectionRange()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= u.DetectionRange)
            return true;

        return false;
    }

    /// <summary>
    /// Puts the patrol points in the order that the unit should traverse them in.
    /// e.g. given patrol points A, B, C, this function change the list to A, B, C, C, B, A
    /// </summary>
    private void SetPatrolPoints()
    {
        List<Transform> points = new List<Transform>(u.PatrolPoints);
        List<Transform> pointsReversed = new List<Transform>(u.PatrolPoints);
        pointsReversed.Reverse();

        List<Transform> mergedList = new List<Transform>();
        mergedList.AddRange(points);
        mergedList.AddRange(pointsReversed);

        patrolPointList = mergedList.ToArray();
    }

    /// <summary>
    /// Pauses the path of the unit and stops it's movement.
    /// Use for when the player is selecting a target and all the enemies need to freeze.
    /// </summary>
    private void Pause()
    {
        isPaused = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero; // So it's an instant stop rather than a slow stop.
    }

    /// <summary>
    /// Resumes the movement of the unit.
    /// Used for when the player is done selecting a target.
    /// </summary>
    private void Resume()
    {
        isPaused = false;
        agent.isStopped = false;
        agent.speed = u.MovementSpeed;
    }

    /// <summary>
    /// Resets the position of the unit and removes any heuristics that were on it.
    /// Used for when the unit gets caught by a guard and the level resets.
    /// </summary>
    private void Reset()
    {
        Destroy(GetComponent<Heuristic>()); // Remove whatever heuristic it's executing.
        transform.position = u.InitialPosition; // Set position to it's initial location.
    }

	private void OnCollisionEnter(Collision collision)
    {
        // Check which team this unit is on.
        if(collision.gameObject.tag == "Projectile") // Got hit by a projectile.
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();

            TakeDamage(p.damage);
            Destroy(collision.gameObject);
        }
    }
}
