using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	-- Alert State --
/// Chases the player until it loses vision. (All alerted guards share vision)
/// If they lose sight of the player, guards play a scan animation.(Look around)
/// </summary>
public class Alert : AIState
{
	private Transform currentTarget;
	private float currentTargetBuffer; // The amount of time the current target can be out of vision before losing it.
	private float outOfVisionDuration; // The amount of time the current target has been out of vision.
    private bool isScanning;
	private bool doneWalking; // If the guard is done walking to the last seen location of the player.
	private IEnumerator scanCoroutine;
	private IEnumerator walkCoroutine;

	public Alert(Enemy enemy): base(enemy)
	{
		GameManager.Instance.PlayerIsMarked = true;
		GameManager.Instance.OutOfVisionDuration = 0;

		enemy.GetComponent<Animator>().SetBool("Alerted", true);
	}

	public override void Tick()
	{
		GameManager.Instance.RoomIsAlerted = true;
		Player player = visionCone.TryGetPlayer();

		if(player != null)
		{
			GameManager.Instance.PlayerIsMarked = true; // Mark the player as the current target.
			GameManager.Instance.OutOfVisionDuration = 0; // Reset the outOfVisionDuration counter if the player is in vision.

			// If the player enters vision again, stop whatever you are doing and go back to normal Alert mode.
			if(scanCoroutine != null)
				enemy.StopCoroutine(scanCoroutine);
			if(walkCoroutine != null)
				enemy.StopCoroutine(walkCoroutine);

			enemy.GetComponent<Animator>().SetBool("Scanning", false);
		}

		if(GameManager.Instance.OutOfVisionDuration >= enemy.EnemySettings.CurrentTargetBuffer) // If target has been out of vision for extended amount of time.
		{
			GameManager.Instance.PlayerIsMarked = false;
		}
			
		if(GameManager.Instance.PlayerIsMarked == false)
		{
			// If this enemy is the closest enemy to to last seen location of the player.
			if(GameObject.ReferenceEquals(GameManager.Instance.GetClosestGuardToPlayer(), enemy))
			{
				if(!doneWalking) // Flag so walk corouting doesn't get called multiple times.
				{
					walkCoroutine = WalkToLastSeenLocation();
					enemy.StartCoroutine(walkCoroutine);
				}
				else // If done walking to last seen location.
				{
					if(!isScanning) // Flag so scan coroutine doesn't get called multiple times.
					{
						scanCoroutine = StartScanPhase(4.0f);
						enemy.StartCoroutine(scanCoroutine);
					}
				}
			}
			else
			{
				if(!isScanning) // Flag so scan coroutine doesn't get called multiple times.
				{
					scanCoroutine = StartScanPhase(4.0f);
					enemy.StartCoroutine(scanCoroutine);
				}
			}
		}
		else
		{
			enemyMovement.MoveTo(GameManager.Instance.Player.transform.position, enemy.EnemySettings.AlertMovementSpeed);
			
			if(enemyAttack.IsInAttackRange(GameManager.Instance.Player.transform.position))
			{
				if(!enemyAttack.IsAttacking)
					enemy.StartCoroutine(enemyAttack.Attack(GameManager.Instance.Player.gameObject));
			}
		}
	}

	public override void LeaveState()
	{
		enemy.GetComponent<Animator>().SetBool("Alerted", false);
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

    /// <summary>
    /// Starts the scan phase.
    /// The guard stands still and scans the area for the player.
    /// </summary>
    /// <param name="duration">How long the scan phase should last.</param>
    /// <returns></returns>
    private IEnumerator StartScanPhase(float duration)
    {
        isScanning = true;
        enemyMovement.Stop();

		// Start the scan phase animation.
		enemy.GetComponent<Animator>().SetBool("Scanning", true);

		// The amount of time the scanning phase lasts.
        yield return new WaitForSeconds(duration); 

		// Return to a Patrol/Idle state.
		GameManager.Instance.RoomIsAlerted = false;

		enemy.GetComponent<Animator>().SetBool("Scanning", false);
		enemy.GetComponent<Animator>().SetBool("Alerted", false);

		if(enemy.HasPatrolPath)
			enemy.ChangeState(new Patrol(enemy));
		else
			enemy.ChangeState(new Idle(enemy));
    }

	private IEnumerator WalkToLastSeenLocation()
	{
		enemy.GetComponent<Animator>().SetBool("AlertWalk", true);
		enemyMovement.MoveTo(VisionCone.LastSeenPlayerLocation, enemy.EnemySettings.DefaultMovementSpeed);

		while(true)
		{
			if (!enemyMovement.Agent.pathPending)
			{
				if (enemyMovement.Agent.remainingDistance <= enemyMovement.Agent.stoppingDistance)
				{
					if (!enemyMovement.Agent.hasPath || enemyMovement.Agent.velocity.sqrMagnitude == 0f)
					{
						// Enemy has reached the location of the last seen location of the player.
						doneWalking = true;
						yield return null;
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}