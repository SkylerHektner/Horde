using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	-- Alert State --
/// Chases the player until it loses vision. (All alerted guards share vision)
/// If they lose sight of the player, guards enter a scan phase.(They look around)
/// The nearest guard to the last scene position will walk to that location and then scan.
/// </summary>
public class Alert : AIState
{
	private static bool playerInVision; 	// Static bool so enemies can share vision of the player.
	private static float outOfVisionBuffer;	// The amount of time the player has been out of vision of all guards.

    private bool isScanning; 				// Flag so scan coroutine doesn't get called multiple times.
	private bool isWalking; 				// Flag so walk coroutine doesn't get called multiple times.
	private IEnumerator scanCoroutine;		// Coroutine that makes guards look around after losing vision of player.
	private IEnumerator walkCoroutine;		// Coroutine for the nearest guard to search the last seen position of player.

	public Alert(Enemy enemy): base(enemy)
	{
		Debug.Log("HIT");
		enemy.GetComponent<Animator>().SetBool("Alerted", true);
	}

	public override void Tick()
	{
		playerInVision = CheckIfPlayerInVision();
			
		if(!playerInVision)
		{
			/* 
							-- Behavior while guard is OUT of vision --
				If this guard is the closest enemy to to last seen location of the player,
				start the Walk coroutine to walk to that position. After arriving to the
				position, the scan coroutine gets called (at the end of the walk coroutine).

				Otherwise (if this guard isn't the closest to the last seen position), just
				start the scan coroutine.
			*/

			if(GameObject.ReferenceEquals(GameManager.Instance.GetClosestGuardToPlayer(), enemy))
			{
				if(!isWalking)
				{
					walkCoroutine = WalkToLastSeenLocation();
					enemy.StartCoroutine(walkCoroutine);
				}
			}
			else // This enemy is not the closest to the last seen position.
			{
				if(!isScanning) 
				{
					scanCoroutine = StartScanPhase(4.0f);
					enemy.StartCoroutine(scanCoroutine);
				}
			}
		}
		else // Player is in vision of the guards.
		{
			/*
								-- Behavior while guard is IN vision --
				(1) Because the player is in vision (maybe re-entering vision), stop whatever 
				the guard is doing and go back to normal Alert mode. (2) While the player is
				in vision, run towards him and attack him if he is in range.
			*/

			// (1)
			if(scanCoroutine != null)
			{
				isScanning = false;
				enemy.StopCoroutine(scanCoroutine);
			}
				
			if(walkCoroutine != null)
			{
				isWalking = false;
				enemy.StopCoroutine(walkCoroutine);
			}
				
			enemy.GetComponent<Animator>().SetBool("Scanning", false);
			enemy.GetComponent<Animator>().SetBool("AlertedWalk", false);

			// (2)
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
		if(scanCoroutine != null)
			enemy.StopCoroutine(scanCoroutine);
		if(walkCoroutine != null)
			enemy.StopCoroutine(walkCoroutine);

		enemy.GetComponent<Animator>().SetBool("Scanning", false);
		enemy.GetComponent<Animator>().SetBool("AlertedWalk", false);
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
	///	When the player enters any guards' vision cone, it resets
	/// the outOfVisionBuffer. This buffer is a counter for how long
	/// the player can be out of vision before the guards stop running
	/// after the player. If the buffer is greater than zero, the
	/// player is in vision of the guards.
	/// /<summary>
	private bool CheckIfPlayerInVision()
	{
		outOfVisionBuffer -= Time.smoothDeltaTime;
		GameManager.Instance.RoomIsAlerted = true;
		
		// Reset the buffer if the player is in vision of a guard.
		Player player = visionCone.TryGetPlayer();
		if(player != null)
		{
			outOfVisionBuffer = enemy.EnemySettings.CurrentTargetBuffer;
		}

		// If the buffer is greater than zero, the player is in vision of the guards.
		if(outOfVisionBuffer > 0)
		{
			return true;
		}

		GameManager.Instance.RoomIsAlerted = false;
		return false;
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
		enemy.GetComponent<Animator>().SetBool("AlertedWalk", false);

		// The amount of time the scanning phase lasts.
		yield return new WaitForSeconds(duration);

		// Scan phase is over so set animations back to normal
		// and change back to Patrol/Idle state.
		GameManager.Instance.RoomIsAlerted = false; 

		enemy.GetComponent<Animator>().SetBool("Scanning", false);
		enemy.GetComponent<Animator>().SetBool("Alerted", false);

		if(enemy.HasPatrolPath)
			enemy.ChangeState(new Patrol(enemy));
		else
			enemy.ChangeState(new Idle(enemy));
    }

	/// <summary>
	///	Coroutine that is called by the closest guard to the last seen position of the player.
	/// Guard will walk to that position and then enter the scan phase.
	/// </summary>
	private IEnumerator WalkToLastSeenLocation()
	{
		isWalking = true;

		enemy.GetComponent<Animator>().SetBool("AlertedWalk", true);
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
						if(!isScanning)
						{
							scanCoroutine = StartScanPhase(4.0f);
							enemy.StartCoroutine(scanCoroutine);
						}
						yield return null;
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}