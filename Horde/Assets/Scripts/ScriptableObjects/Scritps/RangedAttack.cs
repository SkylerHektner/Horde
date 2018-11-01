using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An implementation of the Attack scriptable object.
/// Allows designers to create and customize their own 
/// Ranged Attacks in the editor.
/// </summary>
[CreateAssetMenu(menuName = "Attack/RangedAttack")]
public class RangedAttack : Attack
{
	[SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }

    [SerializeField, Range(10.0f, 80.0f)]
    private float trajectoryAngle;
    public float TrajectoryAngle { get { return trajectoryAngle; } }

    [SerializeField]
    private Transform projectilePrefab;
    public Transform ProjectilePrefab { get { return projectilePrefab; } }

    private Unit unit;

    public override void Initialize(GameObject obj)
    {
        unit = obj.GetComponent<Unit>();
        
        unit.AttackDamage = AttackDamage;
        unit.AttackCooldown = AttackCooldown;
        unit.AttackRange = AttackRange;
        unit.TrajectoryAngle = TrajectoryAngle;
        unit.ProjectilePrefab = ProjectilePrefab;
    }
}
