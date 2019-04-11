using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class AIState
{
	public static event Action<string> OnEmotionStarted 	= (emotionName) => { };
	public static event Action<string> OnEmotionEnded 	= (emotionName) => { };

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
	}

	public virtual void InitializeState()
	{
		UpdateVisionCone();
		UpdateTargetMask();

		enemyMovement.Stop();

		if(enemy.GetCurrentState() != null)
			OnEmotionStarted(enemy.GetCurrentState().ToString());
	}

	public virtual void Tick()
	{
		AIState state = enemy.GetCurrentState();
		if(state is Idle || state is Patrol || state is Anger || state is Alert)
		{
			if(visionCone.TryGetPlayer())
				enemy.ActivateRecIcon();
			else
				enemy.DeactivateRecIcon();
		}
		

		
		duration -= Time.smoothDeltaTime;
		
		if(duration <= 0)
		{
			if(enemy.HasPatrolPath)
				enemy.ChangeState(new Patrol(enemy));
			else
				enemy.ChangeState(new Idle(enemy));
		}
	}

	public virtual void LeaveState()
	{
		OnEmotionEnded(enemy.GetCurrentState().ToString());
		enemy.DeactivateRecIcon();
		enemyMovement.Stop();
	}

	protected abstract void UpdateVisionCone();
	protected abstract void UpdateTargetMask();
}
