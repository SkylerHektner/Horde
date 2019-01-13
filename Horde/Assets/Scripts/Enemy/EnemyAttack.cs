using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour 
{
	public float AttackRange { get { return attackRange; } }

	private float attackRange;

	private void Start()
	{
		attackRange = GetComponent<Enemy>().EnemySettings.AttackRange;
	}

	public void Attack(GameObject target)
	{
		IBreakable breakableObject = target.GetComponent<IBreakable>();
		if(breakableObject != null) // If target is a breakable object.
		{
			breakableObject.Break();
			return;
		}

		// TODO: Add support to attack other enemies and the player.
	}

	public bool IsInAttackRange(Vector3 targetPos)
	{
		if(Vector3.Distance(transform.position, targetPos) <= attackRange)
			return true;

		return false;
	}
}
