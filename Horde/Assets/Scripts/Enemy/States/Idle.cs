using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
	private VisionCone vc;

	public Idle(Enemy enemy, EnemySettings enemySettings): base(enemy, enemySettings)
	{
		vc = enemy.GetComponent<VisionCone>();
		vc.ChangeColor(enemySettings.DefaultColor);
	}

	public override void LeaveState()
	{

	}

	public override void Tick()
	{
		if(vc.VisibleTargets.Count > 0) // If the player is in the vision cone.
		{
			
		}
	}
}
