using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// An implementation of the Attack scriptable object.
/// Allows designers to create and customize their own 
/// Ranged Attacks in the editor.
///
/// </summary>
[CreateAssetMenu(menuName = "Attack/RangedAttack")]
public class RangedAttack : Attack
{
	[SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }

    [SerializeField]
    private float trajectoryAngle;
    public float TrajectoryAngle { get { return trajectoryAngle; } }

    [SerializeField]
    private Transform projectilePrefab;
    public Transform ProjectilePrefab { get { return projectilePrefab; } }
}
