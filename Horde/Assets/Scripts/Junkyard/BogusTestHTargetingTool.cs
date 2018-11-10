using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BogusTestHTargetingTool : MonoBehaviour {

    [ContextMenu("Test Targeting Unit")]
    public void TestTargetingUnit()
    {
        HTargetingTool.Instance.GetTarget(GetComponent<Unit>(), unitCallback);
    }

    [ContextMenu("Test Targeting Pos")]
    public void TestTargetingPos()
    {
        HTargetingTool.Instance.GetPositon(GetComponent<Unit>(), posCallback);
    }

    private void unitCallback(Unit u)
    {

    }

    public void posCallback(Vector3 pos)
    {
        GetComponent<NavMeshAgent>().SetDestination(pos);
    }
}
