using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class H_Move : Heuristic
{
	private Vector3 targetPosition;
    private NavMeshAgent agent;
    private bool moving = false;

	public override void Init()
	{
		base.Init();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);
        HTargetingTool.Instance.GetPositon(unit, positionReady, "Please click where you want this unit to move");
	}

	public override void Execute()
	{
        base.Execute();
        if(moving)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                Resolve();
            }
        }
	}

    private void positionReady(Vector3 pos)
    {
        agent.SetDestination(pos);
        moving = true;
    }
}
