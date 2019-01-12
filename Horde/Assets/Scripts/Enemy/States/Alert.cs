using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : AIState
{
	public Alert(Enemy enemy): base(enemy)
	{
		visionCone = enemy.GetComponent<VisionCone>();
		visionCone.ChangeColor(enemy.EnemySettings.AlertColor);
	}

	public override void Tick()
	{
		enemyMovement.MoveTo(player.transform.position);
	}

	public override void LeaveState()
	{

	}

	
}