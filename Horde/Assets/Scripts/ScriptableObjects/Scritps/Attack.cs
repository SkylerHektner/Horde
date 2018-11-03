using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base clase that Melee Attack and Ranged attack inherit from.
/// </summary> 
public abstract class Attack : ScriptableObject
{
	[SerializeField]
	protected AudioClip soundEffect; // Not used yet.

	[SerializeField]
	protected int attackDamage;

	[SerializeField]
	protected float attackCooldown;

	public abstract void Initialize(Unit u);
	public abstract void ExecuteAttack(Unit u);
}
