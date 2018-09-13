using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// --Seek Heuristic--
/// 
/// Walks straight forward until it see's an enemy.
/// Movement stops upon seeing enemy.
/// </summary>
public class H_AttackNormal : Heuristic
{
    [SerializeField]
    private float attackSpeed = 1.0f;

    [SerializeField]
    private float attackRange = 3f;

    private Unit enemy;

    [SerializeField]
    private float attackTime = 1.0f;

    private bool attackInProgress;

    public override void Init() // Initializing the behavior.
    {
        base.Init();
        enemy = unit.currentTarget;
    }

    public override void Execute() // Logic that should be called every tick.
    {
        if (!attackInProgress)
        {
            StartCoroutine(AttackAnim());
            enemy.TakeDamage(1);
        }
    }

    public override void Resolve() // Exiting the behavior.
    {
        //Set code for after target dies

        base.Resolve();
    }

    IEnumerator AttackAnim()
    {
        attackInProgress = true;
        var initialPosition = transform.position;
        transform.position = enemy.transform.position;
        yield return new WaitForSeconds(attackTime);
        transform.position = initialPosition;
        yield return new WaitForSeconds(attackSpeed);
        attackInProgress = false;

    }
}
