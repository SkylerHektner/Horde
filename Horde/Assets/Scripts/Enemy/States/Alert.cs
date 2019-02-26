using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	-- Alert State --
/// Chases the player until it loses vision. (All alerted guards share vision)
/// If player is still out of vision, guards play a search animation.(Look around)
/// </summary>
public class Alert : AIState
{
	private Transform currentTarget;
	private float currentTargetBuffer; // The amount of time the current target can be out of vision before losing it.
	private float outOfVisionDuration; // The amount of time the current target has been out of vision.
    private bool isScanning;

	public Alert(Enemy enemy): base(enemy)
	{
		GameManager.Instance.PlayerIsMarked = true;
		GameManager.Instance.OutOfVisionDuration = 0;
	}

	public override void Tick()
	{
		Player player = visionCone.TryGetPlayer();

		if(player != null)
		{
			GameManager.Instance.PlayerIsMarked = true; // Mark the player as the current target.
			GameManager.Instance.OutOfVisionDuration = 0; // Reset the outOfVisionDuration counter if the player is in vision.
		}

		if(GameManager.Instance.OutOfVisionDuration >= enemy.EnemySettings.CurrentTargetBuffer) // If target has been out of vision for extended amount of time.
		{
			GameManager.Instance.PlayerIsMarked = false;
		}
			
		if(GameManager.Instance.PlayerIsMarked == false)
		{
			if(!enemyAttack.IsAttacking) // And if the enemy isn't attacking.
			{
                if (enemy.HasPatrolPath)
                {
                    if(!isScanning)
                        enemy.StartCoroutine(StartScanPhase(new Patrol(enemy), 4.0f));
                }
                    
				else
                {
                    if(!isScanning)
                        enemy.StartCoroutine(StartScanPhase(new Idle(enemy), 4.0f));
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
    /// <param name="state">State to transition to after the scan phase.</param>
    /// <param name="duration">How long the scan phase should last.</param>
    /// <returns></returns>
    private IEnumerator StartScanPhase(AIState state, float duration)
    {
        isScanning = true;

        enemyMovement.Stop();

        // Exit the scanning phase if player enters vision again.
        if (GameManager.Instance.PlayerIsMarked)
        {
            isScanning = false;
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        Debug.Log("REACHED");
        enemy.ChangeState(state);
    }
}