﻿using System.Collections;
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
    private bool inRange = false;
    private NavMeshAgent agent;
    private bool attackExecuted = false;

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
            
        //  Follow the enemy if it is moving.
        if (Vector3.Distance(transform.position, unit.CurrentTarget.transform.position) > unit.AttackRange)
        {
            inRange = false;
            agent.isStopped = false;
            agent.SetDestination(unit.CurrentTarget.transform.position);
        }
        else
        {
            inRange = true;

            if (attackExecuted == false)
            {
                StartCoroutine(RangedAttack());
                attackExecuted = true;
            }

            // Stop the unit's movement.
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }

    /// <summary>
    /// Fires a projectile towards the current target.
    /// </summary>
    private IEnumerator RangedAttack()
    {
        GameObject projectileGO;

        // Check if this unit is team one or two so we know which type of projectile to instantiate.
        if (gameObject.tag == "TeamOneUnit")
            projectileGO = Instantiate(Resources.Load("TeamOneProjectile"), transform.position, transform.rotation) as GameObject;
        else
            projectileGO = Instantiate(Resources.Load("TeamTwoProjectile"), transform.position, transform.rotation) as GameObject;

        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        Vector3 normalizedAttackDirection = (unit.CurrentTarget.transform.position - transform.position).normalized;

        instance.velocity = normalizedAttackDirection * 8f;

        Destroy(instance.gameObject, 3);

        // Attack needs a cooldown or else it will resolve way too fast, creating an insane attack speed.
        yield return new WaitForSeconds(unit.AttackCooldown);

        Resolve();
    }
}