using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour 
{
	public float AttackRange { get { return attackRange; } }
	public bool IsAttacking { get { return isAttacking; } }

	private float attackRange;
	private bool isAttacking;
	private Animator animator;

	private void Start()
	{
		attackRange = GetComponent<Enemy>().EnemySettings.AttackRange;
		animator = GetComponent<Animator>();
	}

	public IEnumerator AttackBreakable(IBreakable target)
	{
		transform.LookAt(target.GetPosition());

		isAttacking = true;
		animator.SetTrigger("Attack");

		yield return new WaitForSeconds(0.75f); // Wait a little bit so it breaks when the attack connects.

		target.Break();

		// TODO: Add code for attacking the player and other enemies.

		yield return new WaitForSeconds(0.75f); // Wait a little bit longer before moving again.

		isAttacking = false;
	}

	public bool IsInAttackRange(Vector3 targetPos)
	{
		if(Vector3.Distance(transform.position, targetPos) <= attackRange)
			return true;

		return false;
	}
}
