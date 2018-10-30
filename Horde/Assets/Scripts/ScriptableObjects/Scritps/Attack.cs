using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ScriptableObject
{
	[SerializeField]
	private int attackDamage;
	public int AttackDamage { get { return attackDamage; } }

	[SerializeField]
	private float attackCooldown;
	public float AttackCooldown { get { return attackCooldown; } }
}
