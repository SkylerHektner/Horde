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

    private GameObject foundTarget;
    private Unit[] enemies;
    private NavMeshAgent agent;

    public override void Init() // Initializing the behavior.
    {
        base.Init();

        GameObject enemyContainer = GameObject.Find("Enemies");
        enemies = enemyContainer.GetComponentsInChildren<Unit>();

        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = visionRadius;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(CalculateClosestEnemy().transform.position);
    }

    public override void Execute() // Logic that should be called every tick.
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed); // Move the unit forward.
    }

    public override void Resolve() // Exiting the behavior.
    {
        // Set speed to zero here?
        // Maybe pass in the found target here?

        speed = 0;

        unit.currentTarget = foundTarget.GetComponent<Unit>();

        base.Resolve(); // Switch to the next heuristic
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Enemy")
        {
            Debug.Log("Enemy Found!");

            foundTarget = obj.gameObject;

            if (unit != null)
            {
                Resolve();
            }
        }
    }

    private Unit CalculateClosestEnemy()
    {
        float closestDistance = Vector3.Distance(enemies[0].transform.position, transform.position);
        Unit closestEnemy = enemies[0];

        foreach(Unit enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if(distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
