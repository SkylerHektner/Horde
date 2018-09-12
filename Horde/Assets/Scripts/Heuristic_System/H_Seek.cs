﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// --Seek Heuristic--
/// 
/// Walks straight forward until it see's an enemy.
/// Movement stops upon seeing enemy.
/// </summary>
public class H_Seek : Heuristic
{
    [SerializeField, Tooltip("How fast the unit can move.")]
    private float speed = 3;

    [SerializeField, Tooltip("How far away the enemy is before the unit can see it.")]
    private float visionRadius = 3f;

    public override void Init() // Initializing the behavior.
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = visionRadius;
    }

    public override void Execute() // Logic that should be called every tick.
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed); // Move the unit forward.
    }

    public override void Resolve() // Exiting the behavior.
    {
        speed = 0;
    }
}
