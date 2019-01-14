using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Anger : AIState
{
	NavMeshPath path;

	public Anger(Enemy enemy): base(enemy)
	{
		visionCone.ChangeColor(enemy.EnemySettings.AngerColor);
	}

	public override void Tick()
	{
		if(player == null) // Player isn't in vision.
			BreakClosestObject();
		if (path.corners != null) 
		{
            if(path.corners.Length > 0) 
			{
                for (int i = 0; i < path.corners.Length-1; i++) 
				{
                	Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red);
                }
            }
        }
	}

	public override void LeaveState()
	{

	}

	private void BreakClosestObject()
	{
		IBreakable target = FindClosestBreakable();
		if(target == null)
		{
			// TODO: Initiate behavior when there aren't any more breakables.
			return;
		}

		enemyMovement.MoveTo(target.GetPosition());

		if(enemyAttack.IsInAttackRange(target.GetPosition()))
			target.Break();
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
			//Debug.Log(b.GetPosition());
            agent.CalculatePath(new Vector3(b.GetPosition().x, 0.0f, b.GetPosition().z), path); // Calculate the NavMesh path to the object
			Debug.Log(path.status); // WHY IS THIS ALWAYS INVALID???

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
