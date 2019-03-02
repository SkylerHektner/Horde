﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : AIState 
{
    public Dead(Enemy enemy) : base(enemy)
    {
		enemyMovement.Stop();
        enemy.GetComponent<Animator>().enabled = false;
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
        visionCone.ChangeRadius(0.0f);
    }
}
