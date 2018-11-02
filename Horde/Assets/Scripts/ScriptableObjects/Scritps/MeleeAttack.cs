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

	public override void Initialize(Unit u)
	{
		u.AttackDamage = attackDamage;
		u.AttackCooldown = attackCooldown;
		u.AttackRange = 2; // Melee attacks can only have a range of 2.
		u.ParticleEffectPrefab = particleEffect;
	}

	/// <summary>
	/// Displays a particle effect and applies damage to the target.
	/// </summary>
	public override void ExecuteAttack(Unit u)
    {
		// Play particle effect
		GameObject meleeEffectGO = Instantiate(particleEffect.gameObject, u.CurrentTarget.transform.position, Quaternion.identity);
		Destroy(meleeEffectGO, 0.5f);

		// Apply damage
		u.CurrentTarget.UnitController.TakeDamage(u.AttackDamage);
    }
}
