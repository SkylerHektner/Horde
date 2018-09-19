using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heuristic - Ranged Attack
/// 
/// Shoot projectiles towards the current target.
/// 
/// (TODO)
/// If the current target is null when this heuristic is initialized,   
/// it will stand idle until an enemy walks near it, and will start
/// attacking it upon entering it's range.
/// </summary>
public class H_AttackRanged : Heuristic
{
    [SerializeField] private float attackVelocity = 15;
    [SerializeField] private float attackRange = 3;
    
    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init();

        if(unit.currentTarget != null)
            InvokeRepeating("Shoot", 0f, 1f);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        if (unit.currentTarget == null) // This should mean the unit died.
        {
            Debug.Log("REACHED");
            EnemyManager.instance.UpdateEnemies(); // Notify the enemy manager to update it's enemy list because an enemy died.
            Resolve();
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
}
