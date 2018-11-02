using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An implementation of the Attack scriptable object.
/// Allows designers to create and customize their own 
/// Melee Attacks in the editor.
/// </summary>
[CreateAssetMenu(menuName = "Attack/MeleeAttack")]
public class MeleeAttack : Attack
{
	[SerializeField]
	private Transform particleEffect;
	public Transform ParticleEffect { get { return particleEffect; } }
	
	private Unit unit;

	public override void Initialize(GameObject obj)
	{
		unit = obj.GetComponent<Unit>();

		unit.AttackDamage = AttackDamage;
		unit.AttackCooldown = AttackCooldown;
	}
}
