using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically only used to hold the damage of the projectile.
///
/// Order of execution: Unit sets the damage of projectile on instantiation -> 
/// 					Projectile holds the correct damage                 -> 
///						Projectile applies damage to target on hit
/// </summary>
public class Projectile : MonoBehaviour 
{
	[HideInInspector] public int damage = 1; // Set to one damage by default.
}											 // Will change depending on who fired it.
