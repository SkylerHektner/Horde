using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// --Seek Heuristic--
/// 
/// Walks straight forward until it see's an enemy.
/// Movement stops upon seeing enemy.
/// </summary>
public class H_AttackNormal : Heuristic
{
    [SerializeField] private float attackVelocity = 10;
    [SerializeField] private float attackRange = 3;

    private bool idle;
    private bool attackFlag; // A flag so InvokeRepeating only gets called once in Execute

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init();

        // If there is no current target, the unit will idle.
        if (unit.currentTarget == null)
            idle = true;
        else
            idle = false;

        if (!idle)
        {
            StartCoroutine("Attack");
        }
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        if (!idle && unit.currentTarget == null) // This should mean the enemy died.
        {
            UnitManager.instance.UpdateUnits(); // Notify the enemy manager to update it's enemy list because an enemy died.
            Resolve();
        }

        if (EnemyInRangeCheck() && idle)
        {
            unit.currentTarget = UnitManager.instance.GetClosestEnemy(transform.position);
            attackFlag = true;
            idle = false;
        }

        if (attackFlag)
        {
            Debug.Log("Enemy In Range");

            StartCoroutine("Attack");
            attackFlag = false;
        }
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        base.Resolve();
    }

    /// <summary>
    /// Fires a projectile towards the current target.
    /// </summary>
    private IEnumerator Attack()
    {
        Vector3 startingPosition = transform.position;
        Vector3 enemyPosition = unit.currentTarget.transform.position;

        while(unit.currentTarget != null) // Attack until the target is dead.
        {
            while (Vector3.Distance(transform.position, enemyPosition) > 1f)
            {
                if (unit.currentTarget == null)
                {
                    break;
                }
                Vector3 currentPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, unit.currentTarget.transform.position, Time.deltaTime * attackVelocity);

                yield return null;
            }

            if (unit.currentTarget != null)
            {
                unit.currentTarget.TakeDamage(1);
            }

            while (Vector3.Distance(transform.position, enemyPosition) < 1.75f)
            {
                if (unit.currentTarget == null)
                {
                    break;
                }
                Vector3 currentPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, Time.deltaTime * attackVelocity);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }
        Resolve();
    }

    /// <summary>
    /// Returns true if there is an enemy within the range of the unit.
    /// </summary>
    /// <returns></returns>
    private bool EnemyInRangeCheck()
    {
        if (UnitManager.instance.EnemyCount == 0)
            return false;

        float distanceToClosestEnemy = Vector3.Distance(transform.position, UnitManager.instance.GetClosestEnemy(transform.position).transform.position);

        if (distanceToClosestEnemy <= attackRange)
            return true;

        return false;
    }
}
