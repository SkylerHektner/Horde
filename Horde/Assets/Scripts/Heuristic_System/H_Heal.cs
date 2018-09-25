using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Heal --
/// 
/// Performs a heal on the current target.
/// 
/// If there isn't a current target, it will resolve.
/// 
/// In the future, the heal should probably act differently
/// depending on the base class.
/// </summary>
public class H_Heal : Heuristic
{
    [SerializeField] private float healVelocity = 10;
    [SerializeField] private float healRange = 3;
    
    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init();


        // If there is no current target, switch to the next heuristic
        if (unit.currentTarget == null)
        {
            Resolve();
            return;
        }

        StartCoroutine(HealAlly());
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Can't think of anything to put here right now.
        // If we change it to heal until ally has full health,
        // we will check here if the current unit has full health.
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        base.Resolve();
    }

    /// <summary>
    /// Not used at the moment. Testing a ranged heal instead.
    /// 
    /// Heals the current target.
    /// </summary>
    private IEnumerator Heal()
    {
        Vector3 startingPosition = transform.position;
        Vector3 allyPosition = unit.currentTarget.transform.position;

        while (unit.currentTarget.currentHealth < unit.currentTarget.maxHealth) // Heal until full health
        {
            while (Vector3.Distance(transform.position, allyPosition) > 1f)
            {
                Debug.Log("Too far to heal");
                if (unit.currentTarget == null)
                {
                    break;
                }
                Vector3 currentPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, unit.currentTarget.transform.position, Time.deltaTime * healVelocity);

                yield return null;
            }

            if (unit.currentTarget != null)
            {
                Debug.Log("Healing in progress");
                unit.currentTarget.HealDamage(1);
                yield return new WaitForSeconds(1f);
            }

            while (Vector3.Distance(transform.position, allyPosition) < 1.75f)
            {
                if (unit.currentTarget == null)
                {
                    break;
                }
                Vector3 currentPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, Time.deltaTime * healVelocity);
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }
        Resolve();
    }

    /// <summary>
    /// Fires a healing projectile torwards the targeted ally.
    /// </summary>
    private IEnumerator HealAlly()
    {
        Vector3 healSpawnLocation = unit.currentTarget.transform.position;
        healSpawnLocation.y += 1f;

        GameObject projectileGO = Instantiate(Resources.Load("HealingProjectile"), healSpawnLocation, transform.rotation) as GameObject;
        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        //Vector3 normalizedAttackDirection = (unit.currentTarget.transform.position - transform.position).normalized;

        //instance.velocity = normalizedAttackDirection * healVelocity;

        Destroy(instance.gameObject, 2);

        yield return new WaitForSeconds(0.5f); // The recharge time of the heal

        Resolve();
    }

}
