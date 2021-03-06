﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement: MonoBehaviour
{
    public NavMeshAgent Agent { get { return agent; } }
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
        //agent.ResetPath();
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
		 enemy.CameraHead.transform.rotation = Quaternion.RotateTowards(enemy.CameraHead.transform.rotation, desiredRotation, Time.deltaTime * 350.0f);
    }

    public void LookAtWithHead(Vector3 pos)
    {
		Vector3 direction = pos - enemy.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        //Transform head = GetComponentInChildren<VisionCone>().transform;
		enemy.CameraHead.transform.rotation = Quaternion.Lerp(enemy.CameraHead.transform.rotation, desiredRotation, 5.0f * Time.deltaTime);
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.time * 1.0f);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(duration); // How long he should stare at the position.

        while(transform.rotation != startingRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.time * 1.0f);

            yield return new WaitForEndOfFrame();
        }

        enemy.IsDistracted = false;
    }

    /// <summary>
    /// Makes the enemy rotate towards and look at the given location.
    /// Used for when there is a noise or something that catches an
    /// enemies attention.
    /// </summary>
    public IEnumerator LookAtWithHeadForDuration(Vector3 pos, float duration)
    {
        enemy.IsDistracted = true;

        Vector3 direction = pos - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        Quaternion startingRotation = enemy.CameraHead.transform.rotation;
        
        while(Vector3.Angle(enemy.CameraHead.transform.forward, direction) >= 1.0f)
        {
            enemy.CameraHead.transform.rotation = Quaternion.RotateTowards(enemy.CameraHead.transform.rotation, desiredRotation, Time.deltaTime * 350.0f);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(duration); // How long he should stare at the position.

        while(enemy.CameraHead.transform.rotation != startingRotation)
        {
            enemy.CameraHead.transform.rotation = Quaternion.RotateTowards(enemy.CameraHead.transform.rotation, startingRotation, Time.deltaTime * 350.0f);

            yield return new WaitForEndOfFrame();
        }

        enemy.IsDistracted = false;
    }

    public void Respawn(Vector3 pos)
    {
        agent.Warp(pos);
    }
}
