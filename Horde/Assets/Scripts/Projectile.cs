using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically only used to hold the damage of the projectile.
///
/// Order of execution: Unit sets the damage of projectile on instantiation -> 
/// 					Projectile holds the correct damage                 -> 
///						Projectile applies damage to target on hit
///
/// Also rotates the object along it's trajectory path.
/// </summary>
public class Projectile : MonoBehaviour 
{
	[HideInInspector] public int damage = 1; // Set to one damage by default.
											 // Will change depending on who fired it.

	[HideInInspector] public Team team; // Which team did this projectile shoot from?	

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// Rotate the projectile along it's projectile path.
		transform.rotation = Quaternion.LookRotation(rb.velocity);
	}					
}

public enum Team { TeamOne, TeamTwo }

