using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIState
{
	public Patrol(Enemy enemy): base(enemy)
	{
		VisionCone vc = enemy.GetComponent<VisionCone>();
		vc.ChangeColor(enemy.EnemySettings.DefaultColor);
	}

	public override void LeaveState()
	{

	}

	public override void Tick()
	{

	}
}
