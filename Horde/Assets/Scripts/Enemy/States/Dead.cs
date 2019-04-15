using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dead : AIState 
{
    public Dead(Enemy enemy) : base(enemy) { }

    public override void InitializeState()
    {
        base.InitializeState();

        enemy.GetComponent<Collider>().enabled = false;
        enemy.GetComponent<Animator>().enabled = false;
        enemy.GetComponent<NavMeshAgent>().enabled = false;
        enemy.SetKinematic(false);
    }

    public override void LeaveState()
    {
        // Probably nothing here because an enemy should never leave the dead state.
    }

	protected override void UpdateTargetMask()
    {
        
    }

    protected override void UpdateVisionCone()
    {
        visionCone.ChangeRadius(0.1f);
    }
}
