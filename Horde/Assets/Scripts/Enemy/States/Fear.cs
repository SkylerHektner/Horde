using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : AIState
{
	public Fear(Enemy enemy): base(enemy)
	{
		
	}

	public override void Tick()
	{
		if(visionCone.TryGetPlayer())
		{
			//Vector3 reverseDirection = (enemy.transform.position - visionCone.TryGetPlayer().transform.position).normalized * 0.25f;
			//Vector3 reversedPosition = Quaternion.Euler(180, 0, 0) * Vector3.forward;

			//Vector3 rotatedVector = Quaternion.Euler(180, 0, 0) * Vector3.forward;

			//enemyMovement.MoveTo(visionCone.TryGetPlayer().transform.position);
			//enemyMovement.MoveInDirection(reverseDirection);


			// Move in the opposite direction of the target.
			// TODO: Run away from nearest target in vision cone.
			Vector3 dir = visionCone.TryGetPlayer().transform.position - enemy.transform.position;
			dir = Quaternion.Euler(180, 0, 0) * dir;

			enemyMovement.MoveTo(dir + enemy.transform.position);
		}
	}
		
	public override void LeaveState()
	{

	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.FearColor);
	}

	protected override void UpdateTargetMask()
	{
		// Targets in the vision cone should be the player and other guards.
		LayerMask playerMask = 1 << LayerMask.NameToLayer("Player");
		LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");
		LayerMask targetMask = playerMask | enemyMask;
		visionCone.ChangeTargetMask(targetMask);
	}
}
