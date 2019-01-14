using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState
{
	protected Enemy enemy;
	protected EnemyMovement enemyMovement;
	protected EnemyAttack enemyAttack;
	protected GameObject player;
	protected VisionCone visionCone;
	protected NavMeshAgent agent;

	public AIState(Enemy enemy)
	{
		this.enemy = enemy;

		enemyMovement = enemy.GetComponent<EnemyMovement>();
		enemyAttack = enemy.GetComponent<EnemyAttack>();
		visionCone = enemy.GetComponent<VisionCone>();
		agent = enemy.GetComponent<NavMeshAgent>();

		GetPlayer();

		visionCone.OnTargetEnteredVision += HandleTargetEnteredVision;
		visionCone.OnTargetExitedVision += HandleTargetExitedVision;
	}

	/// <summary>
	///	Loops through the visible targets to find the player.
	/// </summary>
	private void GetPlayer()
	{
		foreach(Transform t in visionCone.VisibleTargets)
		{
			if(t.gameObject.layer == LayerMask.NameToLayer("Player")) // TODO: Make player script so we can use typeof rather than checking the layer
			{
				player = t.gameObject;
				break;
			}
		}
	}

	public abstract void Tick();
	public abstract void LeaveState();
	protected abstract void HandleTargetEnteredVision();
	protected abstract void HandleTargetExitedVision();
}
