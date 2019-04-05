using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Enemy))]
public class EnemyAttack : MonoBehaviour 
{
	public float AttackRange { get { return attackRange; } }
	public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }

	private float attackRange;
	private bool isAttacking;
	private Animator animator;
    GameObject Player;
    private Enemy enemy;
	private float attackCooldown;


	private void Start()
	{
		attackRange = GetComponent<Enemy>().EnemySettings.AttackRange;
		animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();
		isAttacking = false;
		attackCooldown = 1.0f;
	}

	public IEnumerator Attack(GameObject target)
	{
		// TODO: Add angry point animation here.

		GetComponent<EnemyMovement>().Stop();

		transform.LookAt(target.transform.position);

		isAttacking = true;
		animator.SetTrigger("Attack");

		if(target.tag == "Player")
        {
            target.GetComponent<Animator>().SetTrigger("Caught");
            target.GetComponent<PlayerMovement>().isDead = true;
            target.gameObject.transform.LookAt(transform.position);
            target.GetComponent<PlayerMovement>().lockMovementControls = true; // Dont let player run away if getting attacked.
        }

		yield return new WaitForSeconds(0.75f); // Create a delay here to wait for the contact of the bat.

        while (enemy.Paused)
            yield return null;

		if(target.tag == "Player") // If they strike the player
        {
            target.GetComponent<Animator>().SetTrigger("Die");

            yield return new WaitForSeconds(2f); // Give the death animation some time to play.
            while (enemy.Paused)
                yield return null;
            target.GetComponent<PlayerMovement>().isDead = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(target); // Just destroy other enemies for now.
			GameManager.Instance.CurrentRoom.Enemies.Remove(target.GetComponent<Enemy>());
        }

        yield return new WaitForSeconds(attackCooldown);

        while (enemy.Paused)
            yield return null;

		isAttacking = false;
	}

	public IEnumerator AttackBreakable(Breakable target)
	{
		// TODO: Add angry point animation here.

		if(target == null) // If the target is already broken.
			yield break;

		GetComponent<EnemyMovement>().Stop();
		
		transform.LookAt(target.transform.position);

		isAttacking = true;
		animator.SetTrigger("Attack");

		yield return new WaitForSeconds(0.75f); // Wait a little bit so it breaks when the attack connects.

		if(target != null)
			target.Break();

		yield return new WaitForSeconds(attackCooldown);

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
