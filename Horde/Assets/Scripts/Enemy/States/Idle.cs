using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
	public Idle(Enemy enemy, EnemySettings enemySettings): base(enemy, enemySettings)
	{
		VisionCone vc = enemy.GetComponent<VisionCone>();
		vc.ChangeColor(enemySettings.DefaultColor);
	}

	public override void LeaveState()
	{

	}

	public override void Tick()
	{

	}
}
