using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TEMPORARY ATTACK CLASS
/// FOR TESTINg PURPOSES
/// </summary>
public class TempAttack : MonoBehaviour
{
    private float attackRange = 3;
    private float attackVelocity = 15;

    private Unit currentTarget;
    private bool currentlyAttacking = false;

	void Start ()
    {
        InvokeRepeating("CheckForPlayersInRange", 0f, 1f);
	}
	
	void Update ()
    {

	}

    private void CheckForPlayersInRange()
    {
        if (currentlyAttacking == true)
            return;

        if (UnitManager.instance.AllyCount == 0)
            return;

        Unit ally = UnitManager.instance.GetClosestAlly(transform.position);

        if (ally == null)
            return;

        if(Vector3.Distance(ally.transform.position, transform.position) <= attackRange)
        {
            currentTarget = ally;

            GameObject projectileGO = Instantiate(Resources.Load("EnemyProjectile"), transform.position, transform.rotation) as GameObject;
            Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

            Vector3 normalizedAttackDirection = (ally.transform.position - transform.position).normalized;

            instance.velocity = normalizedAttackDirection * attackVelocity;

            Destroy(instance.gameObject, 2);
            //Debug.Log(ally.currentHealth);
            ally.TakeDamage(1);

            if (currentTarget == null) // This should mean it killed the ally.
                currentlyAttacking = false;
        }
    }
}
