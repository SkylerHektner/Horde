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

	public IEnumerator Attack(GameObject target)
	{
		GetComponent<EnemyMovement>().Stop();

		transform.LookAt(target.transform.position);

		isAttacking = true;
		animator.SetTrigger("Attack");

		if(target.tag == "Player")
			target.GetComponent<PlayerMovement>().lockMovementControls = true; // Dont let player run away if getting attacked.
		else
		{
			target.GetComponent<Enemy>().ChangeState(new Idle(target.GetComponent<Enemy>()));
			target.GetComponent<EnemyMovement>().Stop();
		}

		yield return new WaitForSeconds(0.75f);

		if(target.tag == "Player")
			target.GetComponent<Player>().Respawn();
		else
			Destroy(target); // Just destroy other enemies for now.

		yield return new WaitForSeconds(0.75f);

		isAttacking = false;
	}

	public IEnumerator AttackBreakable(IBreakable target)
	{
		GetComponent<EnemyMovement>().Stop();
		
		transform.LookAt(target.GetPosition());

		isAttacking = true;
		animator.SetTrigger("Attack");

		yield return new WaitForSeconds(0.75f); // Wait a little bit so it breaks when the attack connects.

		if(target != null)
			target.Break();

		yield return new WaitForSeconds(0.75f); // Wait a little bit longer before moving again.

		isAttacking = false;
	}

	public bool IsInAttackRange(Vector3 targetPos)
	{
		//Debug.Log(Vector3.Distance(transform.position, targetPos));
		if(Vector3.Distance(transform.position, targetPos) <= attackRange)
			return true;

		return false;
	}
}
