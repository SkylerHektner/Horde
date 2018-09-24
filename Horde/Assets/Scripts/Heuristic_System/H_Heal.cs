using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Heal : Heuristic
{
    [SerializeField] private float healVelocity = 10;
    [SerializeField] private float healRange = 3;



    public override void Init() // --Initializing the behavior.-- //
    {
        Debug.Log("Heal Initatied");
        base.Init();

    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        if ((unit.currentTarget.currentHealth == unit.currentTarget.maxHealth)) // This should mean the enemy died.
        {
            Debug.Log("Target has max health");
            Resolve();
        }
        else
        {
            StartCoroutine("Heal");
        }

    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        base.Resolve();
    }

    /// <summary>
    /// Heals the current target.
    /// </summary>
    private IEnumerator Heal()
    {
        Vector3 startingPosition = transform.position;
        Vector3 allyPosition = unit.currentTarget.transform.position;

        while (unit.currentTarget.currentHealth < unit.currentTarget.maxHealth) // Heal unitl full health
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

}
