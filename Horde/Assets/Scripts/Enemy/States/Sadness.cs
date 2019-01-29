using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sadness : AIState 
{
	private LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");

	public Sadness(Enemy enemy, float duration): base(enemy, duration)
	{
		Collider[] enemies = Physics.OverlapSphere(enemy.transform.position, 15f, enemyMask);

		// Make enemies look at the crying guard.
		foreach(Collider c in enemies)
		{
			if(c.transform == enemy.transform) // Don't count itself.
				continue;

			Enemy _enemy = c.GetComponent<Enemy>();
			if(_enemy.GetCurrentState() is Idle) // Only affect idle guards for now.
			{
				EnemyMovement enemyMovement = c.GetComponent<EnemyMovement>();
				c.GetComponent<Enemy>().StartCoroutine(enemyMovement.LookAtForDuration(enemy.transform.position, duration));
			}
			
		}
	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.SadnessColor);
		visionCone.ChangeRadius(enemy.EnemySettings.SadnessVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.SadnessVisionConeViewAngle);
        visionCone.ChangePulseRate(0.15f);
	}

	protected override void UpdateTargetMask()
	{
		
	}
}
