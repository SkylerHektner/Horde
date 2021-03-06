﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An implementation of the Attack scriptable object.
/// Allows designers to create and customize their own 
/// Ranged Attacks in the editor.
/// </summary>
[CreateAssetMenu(menuName = "Attack/RangedAttack")]
public class RangedAttack : Attack
{
	[SerializeField]
    private float attackRange;

    [SerializeField]
    private Transform projectilePrefab;

    [SerializeField, Range(10.0f, 80.0f)]
    private float trajectoryAngle;

    public override void Initialize(Unit u)
    {
        u.AttackDamage = attackDamage;
        u.AttackCooldown = attackCooldown;
        u.AttackRange = attackRange;
    }

    /// <summary>
    /// Fires a projectile towards the current target.
    /// Used this as reference for the physics formulas:
    ///     https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/#rotationfix
    /// </summary>
    public override void ExecuteAttack(Unit u)
    {
        Vector3 projectileSpawnPoint = u.ProjectileSpawn.transform.position;
        Vector3 targetPosition = u.CurrentTarget.transform.position;

        Vector3 projectileSpawnXZPos = new Vector3(projectileSpawnPoint.x, 0, projectileSpawnPoint.z);
        Vector3 targetXZPos = new Vector3(targetPosition.x, 0, targetPosition.z);

        //unit.projectileSpawn.transform.LookAt(targetXZPos);

       // Shorthands for the formula.
        float R = Vector3.Distance(projectileSpawnXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(trajectoryAngle * Mathf.Deg2Rad);
        float H = targetPosition.y - projectileSpawnPoint.y; 

        // Calculate the local space components of the velocity.
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        // Create the velocity vector in local space and get it in global space.
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = u.transform.TransformDirection(localVelocity);

        GameObject projectileGO;

        projectileGO = Instantiate(projectilePrefab.gameObject, projectileSpawnPoint, Quaternion.identity) as GameObject;

        Projectile p = projectileGO.GetComponent<Projectile>();
        // Set the damage of the projectile.
        p.damage = u.AttackDamage;

        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        //Debug.Log(globalVelocity);
        instance.velocity = globalVelocity;

        Destroy(instance.gameObject, 10);
    }
}
