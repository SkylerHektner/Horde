using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState
{
	protected Enemy enemy;
	protected EnemyMovement enemyMovement;
	protected EnemyAttack enemyAttack;
	protected VisionCone visionCone;
	protected NavMeshAgent agent;

	public AIState(Enemy enemy)
	{
		this.enemy = enemy;

		enemyMovement = enemy.GetComponent<EnemyMovement>();
		enemyAttack = enemy.GetComponent<EnemyAttack>();
		visionCone = enemy.GetComponent<VisionCone>();
		agent = enemy.GetComponent<NavMeshAgent>();

		UpdateVisionConeColor();
		UpdateTargetMask();

		//GetPlayer();
	}

	public abstract void Tick();
	public abstract void LeaveState();
	protected abstract void UpdateVisionConeColor();
	protected abstract void UpdateTargetMask();
}
