using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class H_SeekWeakAlly : Heuristic {

    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the ally is before the unit can see it.")]
    private float visionRadius = 3f;

    private NavMeshAgent agent;
    private Unit weakestAlly;

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init(); // Sets unit var to current unit the heuristic is on

        unit.currentTarget = null;
        agent = GetComponent<NavMeshAgent>();

        weakestAlly = UnitManager.instance.GetWeakestAlly(transform.position);

        agent.SetDestination(weakestAlly.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Seting the destination of the navmesh in the init takes care of
        // the movement for us.
        if (weakestAlly == null)
        {
            weakestAlly = UnitManager.instance.GetWeakestAlly(transform.position);
            
        }
        agent.SetDestination(weakestAlly.transform.position);

        if (Vector3.Distance(transform.position, weakestAlly.transform.position) < visionRadius)
            Resolve();
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
