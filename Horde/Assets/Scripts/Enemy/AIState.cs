using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
	protected Enemy enemy;
	protected EnemyMovement enemyMovement;

	public AIState(Enemy enemy, EnemyMovement enemyMovement)
	{
		this.enemy = enemy;
		this.enemyMovement = enemyMovement;
	}

	public abstract void Tick();
	public abstract void LeaveState();
}
