using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// --[ Anger State ]--
/// Activated when shot with an anger dart or affected from an exploding anger drum.
/// The guard enters a frenzy and started attacking everything.
/// His rage follows a couple rules:
/// 	- The player is prioritized over breakables and other guards. So if the player
///   	  enters vision, the guard will drop whatever his current target is and
///       run after the player.
/// 	- Breakables and guards have the same priority.
/// 	- Has an innate knowledge of where breakables and other guards are. Does not
///       have any knowledge of where the player is unless he enters vision.
/// </summary>
public class Anger : AIState
{
	private Transform currentTarget;
	private float outOfVisionBuffer = 2.0f; 	// The amount of time the current target can be out of vision before losing him.
	private float outOfVisionDuration; 			// The amount of time the current target has been out of vision.
	private Player player;

	public Anger(Enemy enemy, float duration): base(enemy, duration) { }

	public override void InitializeState()
	{
		base.InitializeState();

		enemy.GetComponent<Animator>().SetBool("Angry", true);

		// TODO: Play animation depending on how he got affected (dart or drum)
	}

    public override void Tick()
	{
		// Don't count down the duration tick until the animation has completed.
		base.Tick();

		player = visionCone.TryGetPlayer();

		// Find a current target. The player takes priority over any current target.
		if(player != null)
		{
			if(HasPathToTarget(player.transform))
				currentTarget = player.transform;
		}

		if(currentTarget == null)
		{
			currentTarget = FindClosestTarget();

			// If there are not more targets...
			if(currentTarget == null)
			{
				enemyMovement.Stop();
				enemy.GetComponent<Animator>().SetBool("Scanning", true);
			}
		}
		else // Guard has a current target.
		{
			enemy.GetComponent<Animator>().SetBool("Scanning", false);
			if(player)
			{
				// Camera head should "lock on" to the player.
				enemy.CameraHead.LookAt(new Vector3(currentTarget.position.x,  4, currentTarget.position.z));
			}
			else
			{
				enemy.CameraHead.localRotation = Quaternion.identity;
			}

			if(!enemyAttack.IsAttacking)
				enemyMovement.MoveTo(currentTarget.position, enemy.EnemySettings.AngerMovementSpeed);

			Breakable breakableObject = currentTarget.GetComponent<Breakable>();

			if(breakableObject != null) // Current target is a breakable object.
			{
				if(enemyAttack.IsInAttackRange(currentTarget.position))
				{
					if(!enemyAttack.IsAttacking)
						enemy.StartCoroutine(enemyAttack.AttackBreakable(breakableObject));

					currentTarget = null;
				}
			}
			else // Current target is the player or another guard.
			{
				if(enemyAttack.IsInAttackRange(currentTarget.position))
				{
					if(!enemyAttack.IsAttacking)
						enemy.StartCoroutine(enemyAttack.Attack(currentTarget.gameObject));

					currentTarget = null;
				}
			}
		}
	}

	public override void LeaveState()
    {
		base.LeaveState();
		
        enemy.GetComponent<Animator>().SetBool("Angry", false);
		enemy.GetComponent<Animator>().SetBool("Scanning", false);

		enemy.CameraHead.localRotation = Quaternion.identity;
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
		// Targets detected by the vision cone should be the player and other guards.
		LayerMask playerMask = 1 << LayerMask.NameToLayer("Player");
		LayerMask enemyMask = 1 << LayerMask.NameToLayer("Enemy");
		LayerMask targetMask = playerMask | enemyMask;
		visionCone.ChangeTargetMask(targetMask);
	}

	/// <summary>
	/// Searches through all the guards / breakables and returns the closest object.
	/// Makes use of FindClosestBreakable() and FindClosestEnemy() functions.
	/// </summary>
	private Transform FindClosestTarget()
	{
		NavMeshPath breakablePath = new NavMeshPath();
		NavMeshPath enemyPath = new NavMeshPath();
		float breakablePathDistance;
		float enemyPathDistance;

		Breakable b = FindClosestBreakable();
		Enemy e = FindClosestEnemy();

		// If there aren't any breakables on the map, just return the closest enemy.
		// Likewise if there aren't any enemies.
		// If there aren't any breakables or enemies, just return nothing.
		if (b == null && e == null)
			return null;
		else if(b == null)
			return e.transform;
		else if (e == null)
			return b.transform;
		

		// Calulate the distance of the path to the closest breakable.
		NavMeshHit hit;
		Vector3 position = b.transform.position;
		NavMesh.SamplePosition(new Vector3(position.x, 0.0f, position.z), out hit, 100.0f, NavMesh.AllAreas);
		agent.CalculatePath(hit.position, breakablePath);
		breakablePathDistance = GetPathDistance(breakablePath.corners);

		// Calculate the distance of the path to the closest enemy.
		agent.CalculatePath(e.transform.position, enemyPath);
		enemyPathDistance = GetPathDistance(enemyPath.corners);

		// Return whichever path is the shortest.
		if(breakablePathDistance > enemyPathDistance)
			return e.transform;
		else
			return b.transform;
	}

	/// <summary>
	/// Searches through all the breakables in the room (any object with the Breakable script on it).
	/// Returns the closest one.
	/// </summary>
	private Breakable FindClosestBreakable()
	{
        NavMeshPath path = new NavMeshPath();

		//var breakablesVar = Object.FindObjectsOfType<MonoBehaviour>().OfType<IBreakable>();
		//List<IBreakable> breakables = breakablesVar.ToList();

		List<Breakable> breakables = Object.FindObjectsOfType<Breakable>().ToList();

        if (breakables.Count == 0)
            return null;

        float closestDistance = 10000f;
        Breakable closestBreakable = null;

        foreach (Breakable b in breakables)
        {
			NavMeshHit hit;
			Vector3 position = b.transform.position;
			NavMesh.SamplePosition(new Vector3(position.x, 0.0f, position.z), out hit, 100.0f, NavMesh.AllAreas);
            agent.CalculatePath(hit.position, path);

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
	/// Searches through all the guards in in the room.
	/// Returns the closest one.
	/// </summary>
	private Enemy FindClosestEnemy()
	{
		NavMeshPath path = new NavMeshPath();

		List<Enemy> enemies = GameManager.Instance.CurrentRoom.Enemies;

		if(enemies.Count <= 1)
		{
			return null;
		}

		float closestDistance = 10000f;
		Enemy closestEnemy = null;

		foreach(Enemy e in enemies)
		{
			if(GameObject.ReferenceEquals(e, enemy) || e == null || e.GetCurrentState() is Dead)
				continue;

            agent.CalculatePath(e.transform.position, path);

			if(path.status == NavMeshPathStatus.PathComplete) // Make sure it's a valid path. (So it doesn't target units in unreachable areas.)
			{
				float distance = GetPathDistance(path.corners);

				if(distance <= closestDistance)
				{
					closestDistance = distance;
					closestEnemy = e;
				}
			}
		}

		return closestEnemy;
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

	private bool HasPathToTarget(Transform t)
	{
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(t.position, path);

		if(path.status == NavMeshPathStatus.PathComplete) 
			return true;

		return false;
	}
}
