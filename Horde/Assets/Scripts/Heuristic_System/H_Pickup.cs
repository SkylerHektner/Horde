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
            if (agent.remainingDistance <= 1f && !agent.pathPending)
            {
                if (unitTarget != null)
                {
                    unitTarget.IsMindControlled = true;
                    unitTarget.GetComponent<NavMeshAgent>().enabled = false;
                    unitTarget.beingCarried = true;
                    unitTarget.transform.SetParent(transform, false);
                    unitTarget.transform.localPosition = carryOffset;
                    unitTarget.transform.rotation = Quaternion.identity;
                    Resolve();
                }
                else if (playerTarget != null)
                {
                    playerTarget.toggleCarryMode();
                    playerTarget.transform.parent = transform;
                    playerTarget.transform.localPosition = carryOffset;
                    playerTarget.transform.rotation = Quaternion.identity;
                    Resolve();
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
}
