using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// -- Heuristic: Mutilate --
/// 
/// Executes the attack of the base class on the current target.
/// Will attack the target until it's dead.
/// 
/// Resolves if the current target is dead/null.
/// </summary>
public class H_Mutilate : Heuristic
{
    private float attackCooldown = 1f;
    private float attackVelocity = 10f;
    private float attackRange = 2f;
    private bool inRange = false;

    private NavMeshAgent agent;

    public override void Init()
    {
        base.Init();
        
        agent = GetComponent<NavMeshAgent>();
        
        if (unit.currentTarget != null) // If the unit is still alive.
        {
            //InvokeRepeating("UpdateTargetPosition", 1f, 0.05f);
            StartCoroutine(StartAttacking());
        } 
        else
        {
            // Check if there are any enemies remaining.
            // Returning if the enemy count is zero prevents the game from hanging.
            if(gameObject.tag == "TeamOneUnit")
            {
                if (UnitManager.instance.TeamTwoUnitCount == 0)
                    return;
            }
            if(gameObject.tag == "TeamTwoUnit")
            {
                if (UnitManager.instance.TeamOneUnitCount == 0)
                    return;
            }

            Resolve();
        } 
    }

    public override void Execute()
    {
        if (unit.currentTarget == null)
            return;


        //  Follow the enemy if it is moving.
        if(Vector3.Distance(transform.position, unit.currentTarget.transform.position) > attackRange)
        {
            inRange = false;
            agent.SetDestination(unit.currentTarget.transform.position);
        }
        else
        {
            inRange = true;

            // Stop the unit's movement.
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }

    private IEnumerator StartAttacking()
    {
        // TODO: Call the correct attack corresponding to the base unit.

        while (unit.currentTarget != null) // Attack until the target is dead.
        {
            if(inRange == true)
                Attack();

            yield return new WaitForSeconds(attackCooldown);
        }

        Resolve(); // Switch to the next heuristic when the target is dead.
    }

    private void Attack()
    {
        GameObject projectileGO;

        // Check if this unit is team one or two so we know which type of projectile to instantiate.
        if (gameObject.tag == "TeamOneUnit")
            projectileGO = Instantiate(Resources.Load("TeamOneProjectile"), transform.position, transform.rotation) as GameObject;
        else
            projectileGO = Instantiate(Resources.Load("TeamTwoProjectile"), transform.position, transform.rotation) as GameObject;


        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        Vector3 normalizedAttackDirection = (unit.currentTarget.transform.position - transform.position).normalized;

        instance.velocity = normalizedAttackDirection * attackVelocity;

        Destroy(instance.gameObject, 2);
    }
}
