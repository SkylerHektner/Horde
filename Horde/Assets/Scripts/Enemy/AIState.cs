using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
	protected Enemy enemy;
	protected EnemyMovement enemyMovement;
	protected GameObject player;
	protected VisionCone visionCone;

	public AIState(Enemy enemy)
	{
		this.enemy = enemy;

		enemyMovement = enemy.GetComponent<EnemyMovement>();
		visionCone = enemy.GetComponent<VisionCone>();

		GetPlayer();
	}

	/// <summary>
	///	Loops through the visible targets to find the player.
	/// </summary>
	private void GetPlayer()
	{
		Debug.Log(visionCone.VisibleTargets.Count);
		foreach(Transform t in visionCone.VisibleTargets)
		{
			if(t.gameObject.layer == LayerMask.NameToLayer("Player")) // TODO: Make player script so we can use typeof rather than checking the layer
			{
				player = t.gameObject;
				break;
			}
		}
		Debug.Log(player);
	}

	public abstract void Tick();
	public abstract void LeaveState();
}
