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
	public string Description { get { return description; } }

	[SerializeField]
	private int maxHealth;
	public int MaxHealth { get { return maxHealth; } }

	[SerializeField]
	private float movementSpeed;
	public float MovementSpeed { get { return movementSpeed; } }

	[SerializeField]
	private float detectionRange;
	public float DetectionRange { get { return detectionRange; }}

	public enum UnitType
	{
		Melee,
		Ranged
	}

	public UnitType unitType;

	private Unit u;

	public void Initialize(Unit u)
	{
		u.MaxHealth = maxHealth;
		u.MovementSpeed = movementSpeed;
		u.DetectionRange = detectionRange;

		// TODO: Hookup enum types
	}
}
