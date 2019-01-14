using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
	public Idle(Enemy enemy): base(enemy)
	{
		visionCone.ChangeColor(enemy.EnemySettings.DefaultColor);

		// Go back to original location.
	}

	public override void Tick()
	{
		
	}

	public override void LeaveState()
	{

	}

	protected override void HandleTargetEnteredVision()
	{
		enemy.ChangeState(new Alert(enemy));
	}

	protected override void HandleTargetExitedVision()
	{
		enemy.ChangeState(new Idle(enemy));
	}
}
