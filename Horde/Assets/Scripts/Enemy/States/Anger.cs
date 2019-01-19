using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Anger : AIState
{
	NavMeshPath path;
	//Animator animator;
	//private bool isAttacking;

	public Anger(Enemy enemy): base(enemy)
	{
		
	}

	public override void Tick()
	{
		if(visionCone.TryGetPlayer() == null) // Player isn't in vision.
			BreakClosestObject();
		else
			enemyMovement.MoveTo(visionCone.TryGetPlayer().transform.position);
	}

	public override void LeaveState()
	{
		// TODO: Save spawn transform to also save the angle.
		enemyMovement.MoveTo(enemy.SpawnPosition);
	}

	protected override void UpdateVisionCone()
	{
		visionCone.ChangeColor(enemy.EnemySettings.AngerColor);
		visionCone.ChangeRadius(enemy.EnemySettings.AngerVisionConeRadius);
		visionCone.ChangeViewAngle(enemy.EnemySettings.AngerVisionConeViewAngle);
	}

	protected override void UpdateTargetMask()
	{
		
	}

	private void BreakClosestObject()
	{
		if(!enemyAttack.IsAttacking)
		{
			IBreakable target = FindClosestBreakable();
			if(target == null)
			{
				// TODO: Initiate behavior when there aren't any more breakables.
				return;
			}

			enemyMovement.MoveTo(target.GetPosition());

			if(enemyAttack.IsInAttackRange(target.GetPosition()))
			{
				enemy.StartCoroutine(enemyAttack.AttackBreakable(target));
			}
		}
	}

	private IBreakable FindClosestBreakable()
	{
		path = new NavMeshPath();

		var breakablesVar = Object.FindObjectsOfType<MonoBehaviour>().OfType<IBreakable>();
		List<IBreakable> breakables = breakablesVar.ToList();

        if (breakables.ToList().Count == 0) // Just return null if there aren't any breakables.
            return null;

        float closestDistance = 10000f;
        IBreakable closestBreakable = breakables[0];

        foreach (IBreakable b in breakables)
        {
            agent.CalculatePath(new Vector3(b.GetPosition().x, 0.0f, b.GetPosition().z), path); // Calculate the NavMesh path to the object

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
