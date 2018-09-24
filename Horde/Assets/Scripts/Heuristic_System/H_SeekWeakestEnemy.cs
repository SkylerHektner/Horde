using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// -- Heuristic: Seek Weakest Enemy --
/// 
/// Uses a nav mesh to navigate to the weakest enemy
/// and marks that enemy as the current target.
/// 
/// Resolves upon reaching target.
/// </summary>
public class H_SeekWeakestEnemy : Heuristic {

    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the enemy is before the unit can see it.")]
    private float visionRadius = 3f;

    private NavMeshAgent agent;
    private Unit weakestEnemy;

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init(); // Sets unit var to current unit the heuristic is on

        agent = GetComponent<NavMeshAgent>();

        if (UnitManager.instance.EnemyCount == 0) // Prevents errors when no enemies are left.
        {
            Resolve();
            return;
        }

        weakestEnemy = UnitManager.instance.GetWeakestEnemy(transform.position);

        agent.SetDestination(weakestEnemy.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Seting the destination of the navmesh in the init takes care of
        // the movement for us.
        if (weakestEnemy == null)
        {
            weakestEnemy = UnitManager.instance.GetWeakestEnemy(transform.position);
            agent.SetDestination(weakestEnemy.transform.position);
        }

        if (UnitManager.instance.EnemyCount == 0) // Prevents errors when no enemies are left.
            Resolve();

        if (Vector3.Distance(transform.position, weakestEnemy.transform.position) < visionRadius)
            Resolve();
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        // Stop the unit's movement.
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.ResetPath();

        unit.currentTarget = weakestEnemy;

        base.Resolve(); // Switch to the next heuristic
    }
}
