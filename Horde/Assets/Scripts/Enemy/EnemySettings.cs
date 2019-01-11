using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Settings", fileName = "EnemyData")]
public class EnemySettings : ScriptableObject
{
	[SerializeField] private float defaultMovementSpeed = 10f;
    [SerializeField] private float angerMovementSpeed = 10f;
    [SerializeField] private float fearMovementSpeed = 10f;
    [SerializeField] private float joyMovementSpeed = 10f;
    [SerializeField] private float sadnessMovementSpeed = 10f;
    [SerializeField] private bool hasPatrolPath = false;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color alertColor;
    [SerializeField] private Color angerColor;
    [SerializeField] private Color fearColor;
    [SerializeField] private Color sadnessColor;
    [SerializeField] private Color joyColor;

    public float DefaultMovementSpeed { get { return defaultMovementSpeed; } }
    public float AngerMovementSpeed { get { return angerMovementSpeed; } }
    public float FearMovementSpeed { get { return fearMovementSpeed; } }
    public float JoyMovementSpeed { get { return joyMovementSpeed; } }
    public float SadnessMovementSpeed { get { return sadnessMovementSpeed; } }
    public bool HasPatrolPath { get { return hasPatrolPath; } }

    public Color DefaultColor { get { return angerColor; } }
    public Color AlertColor { get { return angerColor; } }
    public Color AngerColor { get { return angerColor; } }
    public Color FearColor { get { return fearColor; } }
    public Color SadnessColor { get { return sadnessColor; } }
    public Color JoyColor { get { return joyColor; } }
}
