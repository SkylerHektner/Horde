using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Attack --
/// 
/// Executes the attack of the base class on the current target.
/// 
/// If there isn't a current target, it will check if any enemies
/// are within it's range.
/// 
/// Resolves if no current target and no enemies in range.
/// </summary>
public class H_Attack : Heuristic
{
    [SerializeField] private float attackVelocity = 10;
    [SerializeField] private float attackRange = 3;

    public override void Init() // --Initializing the behavior.-- //
    {
        // TODO: Check which base class it is so we know which attack
        //       so we know which attack function to exectue.
        //
        //       (Attack or Ranged Attack)
        base.Init();

        if (unit.currentTarget != null)
        {
            StartCoroutine(RangedAttack());
        }
        else
        {
            if (EnemyInRangeCheck() == true)
            {
                unit.currentTarget = UnitManager.instance.GetClosestEnemy(transform.position);
            }
            else
            {
                Resolve();
            }
        }
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {

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
            yield return new WaitForSeconds(0.2f); // Time before next attack.
        }

        Resolve();
    }

    /// <summary>
    /// Fires a projectile towards the current target.
    /// </summary>
    private IEnumerator RangedAttack()
    {
        while (unit.currentTarget != null) // Attack until the target it dead.
        {
            GameObject projectileGO = Instantiate(Resources.Load("Projectile"), transform.position, transform.rotation) as GameObject;
            Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

            Vector3 normalizedAttackDirection = (unit.currentTarget.transform.position - transform.position).normalized;

            instance.velocity = normalizedAttackDirection * attackVelocity;

            Destroy(instance.gameObject, 2);

            yield return new WaitForSeconds(1f); // How long between shooting each projectile.
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
