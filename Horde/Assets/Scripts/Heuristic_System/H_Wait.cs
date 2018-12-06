using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class H_Wait : Heuristic
{
    private float waitTime = float.MaxValue;
    private float currentTime;

    public override void Init()
    {
        base.Init();
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        //HTargetingTool.Instance.GetInt(unit, waitTimeReady, "How long do you want this unit to wait for?");
        waitTime = 5f;
    }

    public override void Execute()
    {
        base.Execute();
        currentTime += Time.deltaTime;
        if(currentTime > waitTime)
        {
            Debug.Log("Wait resolved");
            Resolve();
        }
    }

    private void waitTimeReady(int time, bool success)
    {
        if(!success)
        {
            Resolve();
            return;
        }

        waitTime = time;
        currentTime = 0;
    }
}
