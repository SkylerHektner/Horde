using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible of executing that main commands of the unit.
/// The Unit class will call most of these functions.
/// </summary>
public class UnitController : MonoBehaviour 
{
    [SerializeField]
    private StatBlock statBlock;

    [SerializeField] 
    private Attack attack;

    [SerializeField]
	private bool isPatrolling; // Set to true if this enemy should be on a patrol path.

	[SerializeField]
	private Transform[] patrolPoints;

    private Unit u;
    private NavMeshAgent agent;
    private int destPoint = 0;

    private void Start()
    {
        u = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();

        if(statBlock == null)
            Debug.LogError("Set the stat block in the Unit Controller!");
        else
            statBlock.Initialize(u); // Initialize all of the unit stats.

        if(attack == null)
            Debug.LogError("Set the Attack in the Unit Controller!");
        else
            attack.Initialize(u); // Initialize all of the attack values.

        if(isPatrolling)
            if(patrolPoints.Length == 0)
                Debug.LogWarning("Unit set to patrol but no patrol points have been set.");

        u.CurrentHealth = u.MaxHealth; // Start the unit with max health.
    }

    private void Update()
    {
        if(isPatrolling)
            if(!agent.pathPending && agent.remainingDistance < 0.01f)
                MoveToNextPatrolPoint();
    }

    private void MoveToNextPatrolPoint()
    {
        if(patrolPoints.Length == 0) // Return if there aren't any patrol points.
            return;

        MoveTo(patrolPoints[destPoint].position);

        destPoint = (destPoint + 1) % patrolPoints.Length; // Increment the index
    }

    public void Attack()
    {
        attack.ExecuteAttack(u);
    }

    /// <summary>
    /// Commands the NavMesh agent to move to the given target.
    /// </summary>
    public void MoveTo(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.speed = u.MovementSpeed;
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
            UnitManager.instance.UpdateUnits();
        }

        return u.CurrentHealth <= 0;
    }
    public bool HealDamage(int dmgAmount)
    {
        if (u.CurrentHealth <= u.MaxHealth)
            u.CurrentHealth += dmgAmount;
 
        return u.CurrentHealth == u.MaxHealth;
    }

	private void OnCollisionEnter(Collision collision)
    {
        // Check which team this unit is on.
        if(gameObject.tag == "TeamOneUnit") // Unit is on team one.
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();

            if(p.team == Team.TeamTwo)
            {
                Destroy(collision.gameObject);
                TakeDamage(p.damage);
            }

            if(collision.gameObject.tag == "Heal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
        else if(gameObject.tag == "TeamTwoUnit") // Unit is on team two.
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();

            if (p.team == Team.TeamOne)
            {
                Destroy(collision.gameObject);
                TakeDamage(p.damage);
            }

            if (collision.gameObject.tag == "Heal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
    }
}
