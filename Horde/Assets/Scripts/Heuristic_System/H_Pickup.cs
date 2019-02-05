using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class H_Pickup : Heuristic
{
    private Unit unitTarget;
    private PlayerMovement playerTarget;
    private NavMeshAgent agent;
    private Vector3 carryOffset = new Vector3(0, 4, 0);

    private bool carrying = false;
    private bool pickedUp = false;
    private Transform originalParent;

    public override void Init()
    {
        base.Init();
        HTargetingTool.Instance.GetUnitOrPlayer(unit, unitReady, "Please click the unit you want to pick up");
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Execute()
    {
        base.Execute();
        if (playerTarget != null || unitTarget != null)
        {
            if (!carrying)
            {
                if (!pickedUp && agent.remainingDistance <= 1.8f && !agent.pathPending)
                {
                    if (unitTarget != null)
                    {
                        originalParent = unitTarget.transform.parent;
                        unitTarget.IsMindControlled = true;
                        unitTarget.GetComponent<NavMeshAgent>().enabled = false;
                        unitTarget.beingCarried = true;
                        unitTarget.transform.SetParent(transform, false);
                        unitTarget.transform.localPosition = carryOffset;
                        unitTarget.transform.rotation = Quaternion.identity;
                    }
                    else if (playerTarget != null)
                    {
                        originalParent = playerTarget.transform.parent;
                        //playerTarget.toggleCarryMode();
                        playerTarget.transform.parent = transform;
                        playerTarget.transform.localPosition = carryOffset;
                        playerTarget.transform.rotation = Quaternion.identity;
                    }
                    pickedUp = true;
                    HTargetingTool.Instance.GetPositon(unit, destReady, "Please click where this unit should set this down");
                }
            }
            else
            {
                if (agent.remainingDistance <= 1.8f && !agent.pathPending)
                {
                    if (unitTarget != null)
                    {
                        unitTarget.IsMindControlled = false;
                        unitTarget.GetComponent<NavMeshAgent>().enabled = true;
                        unitTarget.beingCarried = false;
                        unitTarget.transform.parent = originalParent;
                        unitTarget.transform.position = unitTarget.transform.position - carryOffset;
                        unitTarget.transform.rotation = Quaternion.identity;
                        Resolve();
                    }
                    else if (playerTarget != null)
                    {
                        //playerTarget.untoggleCarryMode();
                        playerTarget.transform.parent = originalParent;
                        playerTarget.transform.position = playerTarget.transform.position - carryOffset;
                        playerTarget.transform.rotation = Quaternion.identity;
                        Resolve();
                    }
                }
            } 
        }
    }

    public void unitReady(object u, bool player, bool success)
    {
        if (!success)
        {
            Resolve();
            return;
        }

        if (player)
        {
            playerTarget = (PlayerMovement)u;
            unit.UnitController.MoveTo(playerTarget.transform.position);
        }
        else
        {
            unitTarget = (Unit)u;
            unit.UnitController.MoveTo(unitTarget.transform.position);
        }
    }

    public void destReady(Vector3 destination, bool success)
    {
        agent.SetDestination(destination);
        carrying = true;
    }
}
