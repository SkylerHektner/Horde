using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour 
{
	public float AttackRange { get { return attackRange; } }
	public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }

	private float attackRange;
	private bool isAttacking;
	private Animator animator;
    GameObject Player;


	private void Start()
	{
		attackRange = GetComponent<Enemy>().EnemySettings.AttackRange;
		animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
	}

	public IEnumerator Attack(GameObject target)
	{
		GetComponent<EnemyMovement>().Stop();

		transform.LookAt(target.transform.position);

		isAttacking = true;
		animator.SetTrigger("Attack");

        Debug.Log(target.tag);

		if(target.tag == "Player")
        {
            target.GetComponent<PlayerMovement>().lockMovementControls = true; // Dont let player run away if getting attacked.
            Player.transform.LookAt(transform.position);
        }

		yield return new WaitForSeconds(0.75f);

		if(target.tag == "Player") // If they strike the player
        {
			// TODO: Start death animation here.

            yield return new WaitForSeconds(.75f); // Give the death animation some time to play.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
		else
			Destroy(target); // Just destroy other enemies for now.

		yield return new WaitForSeconds(0.75f);

		isAttacking = false;
	}

	public IEnumerator AttackBreakable(IBreakable target)
	{
		if(target == null) // If the target is already broken.
			yield break;

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
		if(Vector3.Distance(transform.position, targetPos) <= attackRange)
			return true;

		return false;
	}
}
