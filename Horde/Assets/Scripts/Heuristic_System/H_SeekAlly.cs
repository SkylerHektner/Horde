using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class H_SeekAlly : Heuristic
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

        if (UnitManager.instance.AllyCount == 0) // Prevents errors when no allies are left.
        {
            Resolve();
            return;
        }

        closestAlly = UnitManager.instance.GetClosestAlly(transform.position);

        agent.SetDestination(closestAlly.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Seting the destination of the navmesh in the init takes care of
        // the movement for us.
        if (closestAlly == null)
        {
            closestAlly = UnitManager.instance.GetClosestAlly(transform.position);
            agent.SetDestination(closestAlly.transform.position);
        }

        if (UnitManager.instance.AllyCount == 0) // Prevents errors when no allies are left.
            Resolve();

        if (Vector3.Distance(transform.position, closestAlly.transform.position) < visionRadius)
            Resolve();
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        // Stop the unit's movement.
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.ResetPath();

        unit.currentTarget = closestAlly;

        base.Resolve(); // Switch to the next heuristic
    }
}
