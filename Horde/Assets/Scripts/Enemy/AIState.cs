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
	protected float duration;

	public AIState(Enemy enemy) : this(enemy, Mathf.Infinity) { }

	public AIState(Enemy enemy, float duration)
	{
		this.enemy = enemy;
		this.duration = duration;
        
		enemyMovement = enemy.GetComponent<EnemyMovement>();
		enemyAttack = enemy.GetComponent<EnemyAttack>();
		visionCone = enemy.GetComponentInChildren<VisionCone>();
		agent = enemy.GetComponent<NavMeshAgent>();


		InitializeState();
		enemyMovement.Stop();
	}

	private void InitializeState()
	{
		UpdateVisionCone();
		UpdateTargetMask();
	}

	public virtual void Tick()
	{
		duration -= Time.smoothDeltaTime;
		
		if(duration <= 0)
			LeaveState();
	}
	
	public virtual void LeaveState()
	{
		//enemyAttack.IsAttacking = false;
		if(enemy.HasPatrolPath)
			enemy.ChangeState(new Patrol(enemy));
		else
			enemy.ChangeState(new Idle(enemy));
	}

	protected abstract void UpdateVisionCone();
	protected abstract void UpdateTargetMask();
}
