using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The container for all of the shared data for a base unit.
/// </summary>
[CreateAssetMenu(fileName = "StatBlock", menuName = "StatBlock")]
public class StatBlock : ScriptableObject
{
	[SerializeField]
	private string description;

	[SerializeField]
	private int maxHealth;

	[SerializeField]
	private float movementSpeed;

	[SerializeField]
	private float detectionRange;

	private Unit u;

	public void Initialize(Unit u)
	{
		u.Description = description;
		u.MaxHealth = maxHealth;
		u.MovementSpeed = movementSpeed;
		u.DetectionRange = detectionRange;
	}
}
