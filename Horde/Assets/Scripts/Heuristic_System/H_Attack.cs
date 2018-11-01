using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// -- Heuristic: Attack --
/// 
/// Executes the attack of the base class on the current target once.
/// 
/// Resolves after it attacks once or if the unit was dead before it could attack.
/// </summary>
public class H_Attack : Heuristic
{
    private NavMeshAgent agent;
    private bool attackExecuted = false;
    private bool facingTarget = false;

    public override void Init()
    {
        // TODO: Check which base class it is so we know which attack
        //       function to exectue.
        //
        //       (Attack or Ranged Attack)

        base.Init();

        agent = GetComponent<NavMeshAgent>();

        if (unit.CurrentTarget == null) // If the target is already dead.
        {
            // Check if there are any enemies remaining.
            // Returning if the enemy count is zero prevents the game from hanging.
            if (gameObject.tag == "TeamOneUnit")
            {
                if (UnitManager.instance.TeamTwoUnitCount == 0)
                    return;
            }
            if (gameObject.tag == "TeamTwoUnit")
            {
                if (UnitManager.instance.TeamOneUnitCount == 0)
                    return;
            }

            Resolve();
        }

        agent.SetDestination(unit.CurrentTarget.transform.position);
    }

    public override void Execute()
    {
        if (unit.CurrentTarget == null)
        {
            Resolve();
            return;
        }

        float distanceFromTarget = Vector3.Distance(transform.position, unit.CurrentTarget.transform.position);
            
        //  Follow the enemy if it is moving.
        if (distanceFromTarget > unit.AttackRange) // Target is out of range.
        {
            agent.isStopped = false;
            agent.SetDestination(unit.CurrentTarget.transform.position);
        }
        else // Target is in range.
        {
            // Stop the unit's movement.
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            if(!facingTarget)
                TurnToTarget(unit.CurrentTarget.transform.position);
            else
            {
                if (attackExecuted == false)
                {
                    StartCoroutine(Attack());
                    attackExecuted = true;
                }   
            }
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }

    /// <summary>
    /// Fires a projectile towards the current target.
    /// Used this as reference for the physics formulas:
    ///     https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/#rotationfix
    /// </summary>
    private IEnumerator Attack()
    {
        Vector3 projectileSpawnPoint = unit.projectileSpawn.transform.position;
        Vector3 targetPosition = unit.CurrentTarget.transform.position;

        Vector3 projectileSpawnXZPos = new Vector3(projectileSpawnPoint.x, 0, projectileSpawnPoint.z);
        Vector3 targetXZPos = new Vector3(targetPosition.x, 0, targetPosition.z);

        unit.projectileSpawn.transform.LookAt(targetXZPos);

       // Shorthands for the formula.
        float R = Vector3.Distance(projectileSpawnXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(unit.TrajectoryAngle * Mathf.Deg2Rad);
        float H = targetPosition.y - projectileSpawnPoint.y; 

        // Calculate the local space components of the velocity.
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        // Create the velocity vector in local space and get it in global space.
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        GameObject projectileGO;

        // Check if this unit is team one or two so we know which type of projectile to instantiate.
        if (gameObject.tag == "TeamOneUnit")
            projectileGO = Instantiate(Resources.Load("TeamOneProjectile"), unit.projectileSpawn.transform.position, Quaternion.identity) as GameObject;
        else
            projectileGO = Instantiate(Resources.Load("TeamTwoProjectile"), unit.projectileSpawn.transform.position, Quaternion.identity) as GameObject;

        // Set the damage of the projectile
        Projectile p = projectileGO.GetComponent<Projectile>();
        p.damage = unit.AttackDamage;

        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        //Debug.Log(globalVelocity);
        instance.velocity = globalVelocity;


        Destroy(instance.gameObject, 10);

        // Attack needs a cooldown or else it will resolve way too fast, creating an insane attack speed.
        yield return new WaitForSeconds(unit.AttackCooldown);

        Resolve();
    }

    private void TurnToTarget(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y; // Lock the y to the unit's y so it only rotates around the y axis.
        transform.LookAt(targetPosition);

        // Calculations to check if the unit is looking in the direction of the target.
        Vector3 directionOfTarget = (unit.CurrentTarget.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(directionOfTarget, transform.forward);

        //Debug.Log(dotProduct);

        if(dotProduct <= 1)
            facingTarget = true;
    }
}