using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Scream : Heuristic {

    private float screamRadius = 20f;

    private List<Unit> affectedUnits = new List<Unit>();

    private float screamDuration = float.MaxValue;
    private float timeSoFar = 0f;

    public override void Init()
    {
        base.Init();
        HTargetingTool.Instance.GetInt(unit, IntReadyCallback, "How long should this unit scream?");
    }

    public override void Execute()
    {
        base.Execute();
        timeSoFar += Time.deltaTime;
        if (timeSoFar > screamDuration)
        {
            Resolve();
        }
    }

    public override void Resolve()
    {
        foreach(Unit u in affectedUnits)
        {
            u.UnitController.IsMindControlled = false;
        }
        base.Resolve();
    }

    private void scream()
    {
        Collider[] nearbyStuff = Physics.OverlapSphere(transform.position, screamRadius);
        foreach (Collider c in nearbyStuff)
        {
            Debug.Log(c.tag);
            if (c.tag == "TeamTwoUnit" && c.gameObject != gameObject)
            {
                affectedUnits.Add(c.GetComponent<Unit>());
            }
        }

        foreach (Unit u in affectedUnits)
        {
            u.UnitController.IsMindControlled = true;
            Vector3 gap = (u.transform.position - transform.position).normalized * 2;
            u.UnitController.MoveTo(transform.position + gap);
        }
    }

    private void IntReadyCallback(int val)
    {
        screamDuration = val;
        scream();
        timeSoFar = 0;
    }
}
