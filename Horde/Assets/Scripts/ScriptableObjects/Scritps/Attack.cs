using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base clase that Melee Attack and Ranged attack inherit from.
/// </summary> 
public abstract class Attack : ScriptableObject
{
	[SerializeField]
	private int attackDamage;
	public int AttackDamage { get { return attackDamage; } }

	[SerializeField]
	private float attackCooldown;
	public float AttackCooldown { get { return attackCooldown; } }

	public abstract void Initialize(GameObject obj);
}
