using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
	public Idle(Enemy enemy): base(enemy)
	{
		visionCone.ChangeColor(enemy.EnemySettings.DefaultColor);
	}

	public override void Tick()
	{
		
	}

	public override void LeaveState()
	{

	}
}
