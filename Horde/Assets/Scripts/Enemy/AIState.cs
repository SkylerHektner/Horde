using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
	protected Enemy enemy;

	public AIState(Enemy enemy, EnemySettings enemySettings)
	{
		this.enemy = enemy;
	}

	public abstract void Tick();
	public abstract void LeaveState();
}
