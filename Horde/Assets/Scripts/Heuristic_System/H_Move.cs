using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class H_Move : Heuristic
{
    private NavMeshAgent agent;
    private bool moving = false;

	public override void Init()
	{
		base.Init();
        agent = GetComponent<NavMeshAgent>();
        HTargetingTool.Instance.GetPositon(unit, positionReady, "Please click where you want this unit to move");
	}

	public override void Execute()
	{
        if(moving)
        {
            if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                Resolve();
            }
        }
	}

    private void positionReady(Vector3 pos, bool success)
    {
        if (!success)
        {
            Resolve();
            return;
        }

        unit.UnitController.MoveTo(pos);
        moving = true;
    }
}
