using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Anger : AIState
{
	private Transform currentTarget;
	private float currentTargetBuffer = 2.0f; // The amount of time the current target can be out of vision before losing it.
	private float outOfVisionDuration; // The amount of time the current target has been out of vision.

	public Anger(Enemy enemy, float duration): base(enemy, duration)
	{
        enemy.GetComponent<Animator>().SetBool("Angry", true);
        enemy.GetComponent<Animator>().SetBool("Happy", false);
        enemy.GetComponent<Animator>().SetBool("Sad", false);
        enemy.GetComponent<Animator>().SetBool("Scared", false);

    }

    public override void Tick()
	{
		base.Tick();

		Transform closestTarget = visionCone.GetClosestTarget();
		if(closestTarget != null && closestTarget != currentTarget)
			currentTarget = closestTarget; // Change aggro to the closer target.
		
		if(currentTarget == null)
		{
			BreakClosestObject();
		}
		else if(closestTarget == null) // If there is a current target but he's out of vision.
		{
			outOfVisionDuration += Time.smoothDeltaTime; // Increase the out-of-vision duration counter.
			if(outOfVisionDuration >= currentTargetBuffer)
			{
				currentTarget = null; // Stop chasing the target if he's been out of vision for too long.
			}
			else 
			{
				enemyMovement.MoveTo(currentTarget.position, enemy.EnemySettings.AngerMovementSpeed); // Move to the target.
				if(enemyAttack.IsInAttackRange(currentTarget.position)) // Attack if it's in range.
				{
					if(!enemyAttack.IsAttacking)
						enemy.StartCoroutine(enemyAttack.Attack(currentTarget.gameObject));
				}
			}
		}
		else // There is a target in vision.
		{
			currentTarget = closestTarget; // Aggro the closest target.
			outOfVisionDuration = 0; // Reset the out-of-vision counter.
		}
	}

	public override void LeaveState()
    {
        enemy.GetComponent<Animator>().SetBool("Angry", false);

        base.LeaveState();
    }

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.AngerColor);
		visionCone.ChangeRadius(enemy.EnemySettings.AngerVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.AngerVisionConeViewAngle);
        visionCone.ChangePulseRate(0.5f);
	}

	protected override void UpdateTargetMask()
	{
		// Targets in the vision cone should be the player and other guards.
		LayerMask playerMask = 1 << LayerMask.NameToLayer("Player");
		LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");
		LayerMask targetMask = playerMask | enemyMask;
		visionCone.ChangeTargetMask(targetMask);
	}

	private void BreakClosestObject()
	{
		if(!enemyAttack.IsAttacking)
		{
			IBreakable target = FindClosestBreakable();
			if(target == null)
			{
                //Initiate behavior when there aren't any more breakables.
                enemy.GetComponent<Animator>().SetBool("Angry", false);
                return;
			}

			enemyMovement.MoveTo(target.GetPosition(), enemy.EnemySettings.AngerMovementSpeed);

			if(enemyAttack.IsInAttackRange(target.GetPosition()))
			{
				enemy.StartCoroutine(enemyAttack.AttackBreakable(target));
			}
		}
	}

	private IBreakable FindClosestBreakable()
	{
        NavMeshPath path = new NavMeshPath();

		var breakablesVar = Object.FindObjectsOfType<MonoBehaviour>().OfType<IBreakable>();
		List<IBreakable> breakables = breakablesVar.ToList();

        if (breakables.ToList().Count == 0) // Just return null if there aren't any breakables.
            return null;

        float closestDistance = 10000f;
        IBreakable closestBreakable = breakables[0];

        foreach (IBreakable b in breakables)
        {
			NavMeshHit hit;
			NavMesh.SamplePosition(new Vector3(b.GetPosition().x, 0.0f, b.GetPosition().z), out hit, 150.0f, NavMesh.AllAreas);
            agent.CalculatePath(hit.position, path); // Calculate the NavMesh path to the object

            if(path.status == NavMeshPathStatus.PathComplete) // Make sure it's a valid path. (So it doesn't target units in unreachable areas.)
            {
                float distance = GetPathDistance(path.corners);

                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestBreakable = b;
                }
            }
        }

        return closestBreakable;
	}

	/// <summary>
    /// Given an array of Vector3's (the corners of the path),
    /// This function will return the total distance of the path.
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    private float GetPathDistance(Vector3[] corners)
    {
        float totalDistance = 0;

        for(int i = 0; i < corners.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(corners[i], corners[i + 1]);
        }

        return totalDistance;
    }

}
