using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement: MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Gives a new destination to the NavMesh Agent.
    /// The agent will figure out the rest.
    /// </summary>
    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public void MoveInDirection(Vector3 dir)
    {
        agent.Move(dir);
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
    public void LookAt(Vector3 pos)
    {
        // TODO: Lerp the angle of the enemy to look at a location.
    }
}
