using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sadness : AIState 
{
	private LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");

	public Sadness(Enemy enemy, float duration): base(enemy, duration) { }

	public override void InitializeState()
	{
		enemy.GetComponent<Animator>().SetBool("Sad", true);
        enemy.GetComponent<Animator>().SetBool("Happy", false);
        enemy.GetComponent<Animator>().SetBool("Scared", false);
        enemy.GetComponent<Animator>().SetBool("Angry", false); 
	}

	public override void Tick()
	{
		base.Tick();

		Collider[] enemies = Physics.OverlapSphere(enemy.transform.position, 15f, enemyMask);

		// Make enemies look at the crying guard.
		foreach(Collider c in enemies)
		{
			if(c.transform == enemy.transform) // Don't count itself.
				continue;

			Enemy e = c.GetComponent<Enemy>();
			if(e.GetCurrentState() is Idle || e.GetCurrentState() is Patrol) // Only affect idle and patrolling guards.
			{
				if(!e.IsDistracted)
				{
					// Distract an enemy for the remaining duration of sadness.
					EnemyMovement enemyMovement = c.GetComponent<EnemyMovement>();
					c.GetComponent<Enemy>().StartCoroutine(enemyMovement.LookAtForDuration(enemy.transform.position, duration));
				}
			}
		}
	}

	public override void LeaveState()
	{
		base.LeaveState();

        enemy.GetComponent<Animator>().SetBool("Sad", false);
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
