using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// -- Heuristic: Seek Nearest Ally --
/// 
/// Uses a nav mesh to navigate to the nearest ally
/// and marks that ally as the current target.
/// 
/// Resolves upon reaching target.
/// </summary>
public class H_SeekNearestAlly : Heuristic
{

    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the ally is before the unit can see it.")]
    private float visionRadius = 3f;

    private NavMeshAgent agent;
    private Unit closestAlly;

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init(); // Sets unit var to current unit the heuristic is on

        agent = GetComponent<NavMeshAgent>();

        if (UnitManager.instance.TeamOneUnitCount == 0) // Prevents errors when no allies are left.
        {
            Resolve();
            return;
        }

        closestAlly = UnitManager.instance.GetClosestEnemy(GetComponent<Unit>());

        agent.SetDestination(closestAlly.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Setting the destination of the navmesh in the init takes care of
        // the movement for us.

        if (closestAlly == null) // The unit must have died before we could get a heal off
        {
            Resolve();
            return;
        }

        // When the ally is within the seek radius
        if (Vector3.Distance(transform.position, closestAlly.transform.position) <= visionRadius)
            Resolve();
        else
            agent.SetDestination(closestAlly.transform.position);
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        // Stop the unit's movement.
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.ResetPath();

        unit.CurrentTarget = closestAlly;

        base.Resolve(); // Switch to the next heuristic
    }
}
