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
    public bool IsMindControlled { get; set; }

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
    private Vector3 initialLocation; // So the static guards can go back to their initial location after chasing the player.
    private GameObject player;
    private DrawDetectionRadius detectionRadius;

    public void InitializeController()
    {
        u = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        detectionRadius = GetComponent<DrawDetectionRadius>();
        initialLocation = transform.position;
        player = PlayerManager.instance.Player;

        if(player == null)
            Debug.Log("Playeris null");

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
        IsMindControlled = false; // Start with default behavior.

        if(patrolPoints != null)
            SetPatrolPoints(); 
    }

    private void Update()
    {
        // We don't want normal behavior to execute if the player is being mind controlled.
        if(IsMindControlled)
            return;

        // If the player isn't mind controlled, execute normal behavior:
        //      * If it's a patrol unit, follow the patrol path.
        //      * If the player enter's the unit's vision, chase it.
        //      * If the unit runs outside of the unit's vision, go back to patrol path.
        //      * If it was a static unit, then go back to initial location.
        if(PlayerInDetectionRange()) // The player entered the detection radius of this unit.
        {
            detectionRadius.SetToDetectioncolor();
            MoveTo(player.transform.position);
            return;
        }

        detectionRadius.SetToDefaultColor();

        if(isPatrolling)
        {
            if(!agent.pathPending && agent.remainingDistance < 0.01f)
                MoveToNextPatrolPoint();
        } 
        else
        {
            MoveTo(initialLocation); // Static unit should move back to it's initial location.
        }   
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
        List<Transform> points = new List<Transform>(patrolPoints);
        List<Transform> pointsReversed = new List<Transform>(patrolPoints);
        pointsReversed.Reverse();

        List<Transform> mergedList = new List<Transform>();
        mergedList.AddRange(points);
        mergedList.AddRange(pointsReversed);

        patrolPoints = mergedList.ToArray();
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
