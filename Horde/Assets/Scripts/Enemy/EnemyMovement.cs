using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement: MonoBehaviour
{
    private Enemy enemy;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 lastPos;
    public bool talking;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;
    }

    private void Update()
    {
        // make them face the way they are walking
        if ((lastPos.x != transform.position.x || lastPos.z != transform.position.z))
        {
            //transform.forward = transform.position - lastPos;
            lastPos = transform.position;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    /// <summary>
    /// Gives a new destination to the NavMesh Agent.
    /// The agent will figure out the rest.
    /// </summary>
    public void MoveTo(Vector3 pos, float speed)
    {
        //Debug.Log(pos);
        agent.ResetPath();
        agent.isStopped = false;
        agent.speed = speed;
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
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void ResumePath()
    {
        agent.isStopped = false;
    }

    public void PausePath()
    {
        agent.isStopped = true;
    }

    public void LookAt(Vector3 pos)
    {
		Vector3 direction = pos - enemy.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        //Transform head = GetComponentInChildren<VisionCone>().transform;
		enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, desiredRotation, 5.0f * Time.deltaTime);
    }

    /// <summary>
    /// Makes the enemy rotate towards and look at the given location.
    /// Used for when there is a noise or something that catches an
    /// enemies attention.
    /// </summary>
    public IEnumerator LookAtForDuration(Vector3 pos, float duration)
    {
        enemy.IsDistracted = true;

        Vector3 direction = pos - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        Quaternion startingRotation = transform.rotation;
        
        while(Vector3.Angle(transform.forward, pos - transform.position) >= 1.0f)
        {
            if(enemy.DEBUG_MODE)
            {
                Debug.Log("Rotating towards.");
                Debug.Log(Vector3.Angle(transform.forward, pos - transform.position));
            }
                
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 3.0f * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(duration); // How long he should stare at the position.

        while(transform.rotation != startingRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startingRotation, 3.0f * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }

        enemy.IsDistracted = false;
    }

    public void Respawn(Vector3 pos)
    {
        agent.Warp(pos);
    }
}
