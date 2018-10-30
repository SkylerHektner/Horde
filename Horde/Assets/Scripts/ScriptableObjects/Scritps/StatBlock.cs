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
	private int maxHealth;
	public int MaxHealth { get { return maxHealth; } }

	[SerializeField]
	private float movementSpeed;
	public float MovementSpeed { get { return movementSpeed; } }

	[SerializeField]
	private string unitType;
	public string UnitType { get { return unitType ; } }

	private Unit u;

	public void Initialize(GameObject obj)
	{
		u = obj.GetComponent<Unit>();

		u.MaxHealth = MaxHealth;
		u.MovementSpeed = MovementSpeed;
	}
}
