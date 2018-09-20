using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heuristic - Ranged Attack
/// 
/// Shoot projectiles towards the current target.
/// 
/// If the current target is null when this heuristic is initialized,
/// it will stand idle until an enemy walks near it, and will start
/// attacking it upon entering it's range.
/// 
/// WARNING: This code is hacky and messy and was written at 3AM.
/// </summary>
public class H_AttackRanged : Heuristic
{
    [SerializeField] private float attackVelocity = 15;
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

        if(!idle)
            InvokeRepeating("Shoot", 0f, 1f);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        if (!idle && unit.currentTarget == null) // This should mean the unit died.
        {
            Debug.Log("REACHED");
            UnitManager.instance.UpdateUnits(); // Notify the enemy manager to update it's enemy list because an enemy died.
            Resolve();
        }

        if (EnemyInRangeCheck() && idle)
        {
            unit.currentTarget = UnitManager.instance.CalculateClosestEnemy(transform.position);
            attackFlag = true;
            idle = false;
        }
            
        if(attackFlag)
        {
            InvokeRepeating("Shoot", 0f, 1f);
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
    private void Shoot()
    {
        GameObject projectileGO = Instantiate(Resources.Load("Projectile"), transform.position, transform.rotation) as GameObject;
        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        Vector3 normalizedAttackDirection = (unit.currentTarget.transform.position - transform.position).normalized;

        instance.velocity = normalizedAttackDirection * attackVelocity;

        Destroy(instance.gameObject, 2);
    }

    private bool EnemyInRangeCheck()
    {
        if (UnitManager.instance.EnemyCount == 0)
            return false;

        float distanceToClosestEnemy = Vector3.Distance(transform.position, UnitManager.instance.CalculateClosestEnemy(transform.position).transform.position);

        if (distanceToClosestEnemy <= attackRange)
            return true;

        return false;
    }
}
