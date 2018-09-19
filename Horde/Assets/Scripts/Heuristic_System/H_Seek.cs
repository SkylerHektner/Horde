using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// --Seek Heuristic--
/// 
/// Walks straight forward until it see's an enemy.
/// Movement stops upon seeing enemy.
/// </summary>
public class H_Seek : Heuristic
{
    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the enemy is before the unit can see it.")]
    private float visionRadius = 3f;

    private NavMeshAgent agent;
    private Unit closestEnemy;

    public override void Init() // --Initializing the behavior.-- //
    {
        base.Init(); // Sets unit var to current unit the heuristic is on

        agent = GetComponent<NavMeshAgent>();

        if (EnemyManager.instance.GetEnemyCount() == 0) // Prevents errors when no enemies are left.
        {
            Resolve();
            return;
        }
        
        closestEnemy = EnemyManager.instance.CalculateClosestEnemy(transform.position);
        
        agent.SetDestination(closestEnemy.transform.position);
    }

    public override void Execute() // --Logic that should be called every tick.-- //
    {
        // Seting the destination of the navmesh in the init takes care of
        // the movement for us.

        if (EnemyManager.instance.GetEnemyCount() == 0) // Prevents errors when no enemies are left.
            Resolve();

        if (Vector3.Distance(transform.position, closestEnemy.transform.position) < visionRadius)
            Resolve();
    }

    public override void Resolve() // --Exiting the behavior.-- //
    {
        // Stop the unit's movement.
        agent.velocity = Vector3.zero; 
        agent.SetDestination(transform.position);

        unit.currentTarget = closestEnemy;

        base.Resolve(); // Switch to the next heuristic
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Enemy")
        {
            //Debug.Log("Enemy Found!");

            unit.currentTarget = obj.gameObject.GetComponent<Unit>(); // Set the current target to the enemy it ran into

            if (unit != null)
            {
                Resolve();
            }
        }
    }
}
