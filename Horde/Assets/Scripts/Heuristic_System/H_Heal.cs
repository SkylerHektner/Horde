using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Heal --
/// 
/// Performs a heal on itself then resolves.
/// 
/// </summary>
public class H_Heal : Heuristic
{
    public override void Init()
    {
        base.Init();

        StartCoroutine(Heal());
    }

    public override void Execute()
    {
        // Can't think of anything to put here right now.
        // If we change it to heal until ally has full health,
        // we can check here if the current unit has full health.
    }

    public override void Resolve()
    {
        base.Resolve();
    }
    
    /// <summary>
    /// Spawns a heal above it's head.
    /// Heals when it falls down onto the unit.
    /// </summary>
    private IEnumerator Heal()
    {
        Vector3 healSpawnLocation = transform.position;
        healSpawnLocation.y += 1f; // The distance you want the heal to spawn over the unit's head.

        GameObject projectileGO = Instantiate(Resources.Load("Heal"), healSpawnLocation, transform.rotation) as GameObject;
        Rigidbody instance = projectileGO.GetComponent<Rigidbody>();

        Destroy(instance.gameObject, 2);

        yield return new WaitForSeconds(1f); // The recharge time of the heal

        Resolve();
    }

}
