using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_WatchForEnemy : Heuristic
{
    private string lookingForTag;

    public override void Init()
    {
        base.Init();
        if (gameObject.tag == "TeamOneUnit")
        {
            lookingForTag = "TeamTwoUnit";
        }
        else
        {
            lookingForTag = "TeamOneUnit";
        }
    }

    public override void Execute()
    {
        base.Execute();
        Collider[] inRange = Physics.OverlapSphere(transform.position, unit.AttackRange);
        foreach(Collider c in inRange)
        {
            if (c.tag == lookingForTag)
            {
                unit.currentTarget = c.GetComponent<Unit>();
                Resolve();
            }
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }
}
