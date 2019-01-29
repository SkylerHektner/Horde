using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : AIState
{
	public Fear(Enemy enemy, float duration): base(enemy, duration)
	{
		
	}

	public override void Tick()
	{
		base.Tick();
		
		if(visionCone.VisibleTargets.Count > 0)
		{
			Transform closestTarget = GetClosestTargetInVisionCone();
			Vector3 runTo = enemy.transform.position + ((enemy.transform.position - closestTarget.position) * 0.5f);
 			float distance = Vector3.Distance(enemy.transform.position, closestTarget.position);
			enemyMovement.MoveTo(runTo, enemy.EnemySettings.FearMovementSpeed);
		}
	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.FearColor);
		visionCone.ChangeRadius(enemy.EnemySettings.FearVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.FearVisionConeViewAngle);
        visionCone.ChangePulseRate(0.8f);
	}

	protected override void UpdateTargetMask()
	{
		// Targets in the vision cone should be the player and other guards.
		LayerMask playerMask = 1 << LayerMask.NameToLayer("Player");
		LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");
		LayerMask targetMask = playerMask | enemyMask;
		visionCone.ChangeTargetMask(targetMask);
	}

	private Transform GetClosestTargetInVisionCone()
	{
		Transform closestTarget = visionCone.VisibleTargets[0];

		foreach(Transform t in visionCone.VisibleTargets)
		{
			if(Vector3.Distance(enemy.transform.position, closestTarget.position) > Vector3.Distance(enemy.transform.position, t.position))
				closestTarget = t;
		}

		return closestTarget;
	}
}
