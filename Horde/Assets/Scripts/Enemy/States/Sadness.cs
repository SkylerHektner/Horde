using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sadness : AIState 
{
	public Sadness(Enemy enemy): base(enemy)
	{
		
	}

	public override void Tick()
	{

	}

	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.SadnessColor);
		visionCone.ChangeRadius(enemy.EnemySettings.SadnessVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.SadnessVisionConeViewAngle);
	}

	protected override void UpdateTargetMask()
	{
		
	}
}
