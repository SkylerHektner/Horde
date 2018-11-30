using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BogusTestHTargetingTool : MonoBehaviour {

    [ContextMenu("Test Targeting Unit")]
    public void TestTargetingUnit()
    {
        HTargetingTool.Instance.GetTarget(GetComponent<Unit>(), unitCallback, 
            "Please select the unit you want this unit to target");
    }

    [ContextMenu("Test Targeting Pos")]
    public void TestTargetingPos()
    {
        HTargetingTool.Instance.GetPositon(GetComponent<Unit>(), posCallback,
            "Please select the position you want this unit to move");
    }

    [ContextMenu("Test Waiting")]
    public void TestGettingInt()
    {
        HTargetingTool.Instance.GetInt(GetComponent<Unit>(), intCallback,
            "Please select how long you want this unit to wait");
    }

    private void unitCallback(Unit u, bool success)
    {

    }

    public void posCallback(Vector3 pos, bool success)
    {
        GetComponent<NavMeshAgent>().SetDestination(pos);
    }

    public void intCallback(int val, bool success)
    {
        Debug.Log(val);
    }
}
