using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joy : AIState
{
	public Joy(Enemy enemy, float duration): base(enemy, duration)
	{
        enemy.GetComponent<Animator>().SetBool("Happy", true);
    }

    public override void Tick()
	{
		base.Tick();
	}

    protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.JoyColor);
		visionCone.ChangeRadius(enemy.EnemySettings.JoyVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.JoyVisionConeViewAngle);
        visionCone.ChangePulseRate(0.4f);
	}

	protected override void UpdateTargetMask()
	{
		
	}
}
