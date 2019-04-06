using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

/// <summary>
///	--[ Alert State ]--
/// Guards enter the Alert state when the players enters one or more of the guards' vision cones.
/// If a guard sees a player during this state, he will run after him and try to attack him.
/// Otherwise, he will stand still and rotate the camera around his head to scan the area.
/// </summary>
public class Alert : AIState
{
	public static event Action OnPlayerEnterVisionCone = delegate { };

	private static float outOfVisionBuffer;	// The amount of time the player has been out of vision of all guards.

	private bool playerInVision;			// If the player is in the guard's vision or not.
	private bool isSpinningHead;			// Flag so spin head coroutine doesn't get called multiple times.
	private bool headIsReset;
	private float outOfVisionDuration;		// The amount of time the player has been out of vision of all the guards.
	private Player player;

	public Alert(Enemy enemy): base(enemy) { }

	public override void InitializeState()
	{
		base.InitializeState();

		// The vision duration counter should reset every time the player enters the vision cone of any guard.
		OnPlayerEnterVisionCone += ResetVisionCounter; 

		player = GameManager.Instance.Player;
		enemy.GetComponent<Animator>().SetBool("Alerted", true);

		GameManager.Instance.RoomIsAlerted = true;
	}

	public override void Tick()
	{
		base.Tick();

		playerInVision = visionCone.TryGetPlayer();

		if(!playerInVision)
		{
			if(!headIsReset)
				ResetHeadRotation();

			enemy.GetComponent<Animator>().SetBool("Spinning", true);
			enemyMovement.Stop();

			SpinHead();
		}
		else // Player IS in vision of the guards.
		{
			OnPlayerEnterVisionCone();

			// If the guard has no path to the player, he should stare at the player to keep him in vision.
			if(!HasPathToTarget(player.transform))
			{
				enemy.CameraHead.LookAt(new Vector3(player.transform.position.x,  4, player.transform.position.z));
				return;
			}

			headIsReset = false;

			enemy.GetComponent<Animator>().SetBool("Spinning", false);

			// Camera head should "lock on" to the target.
			enemy.CameraHead.LookAt(new Vector3(player.transform.position.x,  4, player.transform.position.z));

			// Run at the player and attack him if he's in vision.
			enemyMovement.MoveTo(GameManager.Instance.Player.transform.position, enemy.EnemySettings.AlertMovementSpeed);
			if(enemyAttack.IsInAttackRange(GameManager.Instance.Player.transform.position))
			{
				if(!enemyAttack.IsAttacking)
					enemy.StartCoroutine(enemyAttack.Attack(GameManager.Instance.Player.gameObject));
			}
		}

		// If the player has been out of vision for x seconds, return back to idle/patrol states.
		outOfVisionDuration += Time.smoothDeltaTime;
		if(outOfVisionDuration >= 4.0f)
		{
			if(enemy.HasPatrolPath)
				enemy.ChangeState(new Patrol(enemy));
			else
				enemy.ChangeState(new Idle(enemy));
		}
	}

	public override void LeaveState()
	{
		base.LeaveState();

		ResetHeadRotation();

		enemy.GetComponent<Animator>().SetBool("Scanning", false);
		enemy.GetComponent<Animator>().SetBool("AlertedWalk", false);
		enemy.GetComponent<Animator>().SetBool("Alerted", false);
		enemy.GetComponent<Animator>().SetBool("Spinning", false);

		GameManager.Instance.RoomIsAlerted = false;
	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.AlertColor);
		visionCone.ChangeRadius(enemy.EnemySettings.AlertVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.AlertVisionConeViewAngle);
        visionCone.ChangePulseRate(.5f);
	}

	protected override void UpdateTargetMask()
	{
		LayerMask targetMask = 1 << LayerMask.NameToLayer("Player");
		visionCone.ChangeTargetMask(targetMask);
	}

	private void ResetVisionCounter()
	{
		outOfVisionDuration = 0;
	}

	private void SpinHead()
	{
		enemy.CameraHead.Rotate(Vector3.up, 100.0f * Time.deltaTime);
	}

	private void ResetHeadRotation()
	{
		enemy.CameraHead.localRotation = Quaternion.identity;
		headIsReset = true;
	}


	private bool HasPathToTarget(Transform t)
	{
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(t.position, path);

		if(path.status == NavMeshPathStatus.PathComplete) 
			return true;

		return false;
	}
}