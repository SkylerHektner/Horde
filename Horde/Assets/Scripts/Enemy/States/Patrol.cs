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

	public override void Tick()
	{

	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.DefaultColor);
		visionCone.ChangeRadius(enemy.EnemySettings.DefaultVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.DefaultVisionConeViewAngle);
	}

	protected override void UpdateTargetMask()
	{
		
	}
}
