using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_AttackRanged : Heuristic
{
    [SerializeField] private float attackSpeed = 15;

    

    public override void Init()
    {
        base.Init();
        InvokeRepeating("Shoot", 0f, 1f);
    }

    public override void Execute()
    {
        if (unit.currentTarget == null)
        {
            Resolve();
        }
            
    }

    public override void Resolve()
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

        instance.velocity = normalizedAttackDirection * attackSpeed;

        Destroy(instance.gameObject, 2);
    }
}
