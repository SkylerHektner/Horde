using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
	protected Enemy enemy;
	protected EnemySettings enemySettings;

	public AIState(Enemy enemy, EnemySettings enemySettings)
	{
		this.enemy = enemy;
		this.enemySettings = enemySettings;
	}

	public abstract void Tick();
	public abstract void LeaveState();
}
