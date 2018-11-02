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
    private NavMeshAgent agent;
    private bool attackExecuted = false;
    private bool facingTarget = false;

    public override void Init()
    {
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
        agent.speed = unit.MovementSpeed;
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
            StopCoroutine(Attack()); // Stop attacking if out of range.
            agent.isStopped = false;
            agent.SetDestination(unit.CurrentTarget.transform.position);
            agent.speed = unit.MovementSpeed;
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
    /// Executes one attack and then resolves.
    /// </summary>
    private IEnumerator Attack()
    {
        while(unit.CurrentTarget != null) // Attack until the target is dead.
        {
            unit.UnitController.Attack();

            // Attack needs a cooldown or else it will resolve way too fast, creating an insane attack speed.
            yield return new WaitForSeconds(unit.AttackCooldown);
        }

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
