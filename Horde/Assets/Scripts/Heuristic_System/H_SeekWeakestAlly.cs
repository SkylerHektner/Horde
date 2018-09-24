using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// -- Heuristic: Seek Weakest Ally --
/// 
/// Uses a nav mesh to navigate to the weakest ally
/// and marks that ally as the current target.
/// 
/// Resolves upon reaching target.
/// </summary>
public class H_SeekWeakestAlly : Heuristic {

    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the ally is before the unit can see it.")]
    private float visionRadius = 3f;

    private NavMeshAgent agent;
    private Unit weakestAlly;

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init(); // Sets unit var to current unit the heuristic is on

        agent = GetComponent<NavMeshAgent>();

        if (UnitManager.instance.AllyCount == 0) // Prevents errors when no allies are left.
        {
            Resolve();
            return;
        }

        weakestAlly = UnitManager.instance.GetWeakestAlly(transform.position);

        agent.SetDestination(weakestAlly.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Setting the destination of the navmesh in the init takes care of
        // the movement for us.

        if (weakestAlly == null) // The unit must have died before we could get a heal off
        {
            Resolve();
            return;
        }

        // When the ally is within the seek radius
        if (Vector3.Distance(transform.position, weakestAlly.transform.position) <= visionRadius)
            Resolve();
        else
            agent.SetDestination(weakestAlly.transform.position);
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        Debug.Log("Weakest Ally Seek resolved");
        // Stop the unit's movement.
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.ResetPath();

        unit.currentTarget = weakestAlly;

        base.Resolve(); // Switch to the next heuristic
    }
}
