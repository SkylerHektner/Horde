using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement: MonoBehaviour
{
    private EnemySettings enemySettings;
    private NavMeshAgent agent;

    public EnemyMovement(EnemySettings enemySettings, NavMeshAgent agent)
    {
        this.enemySettings = enemySettings;
        this.agent = agent;
    }

    /// <summary>
    /// Gives a new destination to the NavMesh Agent.
    /// The agent will figure out the rest.
    /// </summary>
    public void MoveTo(Vector3 pos)
    {

    }

    /// <summary>
    /// Halts the movement of the NavMesh Agent.
    /// </summary>
    public void Stop()
    {
        
    }

    /// <summary>
    /// Makes the enemy rotate towards and look at the given location.
    /// Used for when there is a noise or something that catches an
    /// enemies attention.
    /// </summary>
    public void RotateTowards(Vector3 pos)
    {

    }
}
