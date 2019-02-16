using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Settings", fileName = "EnemyData")]
public class EnemySettings : ScriptableObject
{
    [Header("Attack Range")]
    [SerializeField] private float attackRange;

    [Header("Movement Speed Settings")]
	[SerializeField] private float defaultMovementSpeed;
    [SerializeField] private float alertMovementSpeed;
    [SerializeField] private float angerMovementSpeed;
    [SerializeField] private float fearMovementSpeed;
    [SerializeField] private float sadnessMovementSpeed;
    [SerializeField] private float joyMovementSpeed;

    [Header("Vision Cone Color Settings")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color alertColor;
    [SerializeField] private Color angerColor;
    [SerializeField] private Color fearColor;
    [SerializeField] private Color sadnessColor;
    [SerializeField] private Color joyColor;

    [Header("Vision Cone Radius Settings")]
    [SerializeField, Range(0, 30)] private float defaultVisionConeRadius;
    [SerializeField, Range(0, 30)] private float alertVisionConeRadius;
    [SerializeField, Range(0, 30)] private float angerVisionConeRadius;
    [SerializeField, Range(0, 30)] private float fearVisionConeRadius;
    [SerializeField, Range(0, 30)] private float sadnessVisionConeRadius;
    [SerializeField, Range(0, 30)] private float joyVisionConeRadius;

    [Header("Vision Cone View Angle Settings")]
    [SerializeField, Range(0, 360)] private float defaultVisionConeViewAngle;
    [SerializeField, Range(0, 360)] private float alertVisionConeViewAngle;
    [SerializeField, Range(0, 360)] private float angerVisionConeViewAngle;
    [SerializeField, Range(0, 360)] private float fearVisionConeViewAngle;
    [SerializeField, Range(0, 360)] private float sadnessVisionConeViewAngle;
    [SerializeField, Range(0, 360)] private float joyVisionConeViewAngle;

    [Header("Misc Settings")]
    [Tooltip("How long the guard looks at you until he triggers the alert state.")]
    [SerializeField] private float preAlertDuration;

    public float AttackRange { get { return attackRange; } }

    public float DefaultMovementSpeed { get { return defaultMovementSpeed; } }
    public float AlertMovementSpeed { get { return alertMovementSpeed; } }
    public float AngerMovementSpeed { get { return angerMovementSpeed; } }
    public float FearMovementSpeed { get { return fearMovementSpeed; } }
    public float SadnessMovementSpeed { get { return sadnessMovementSpeed; } }
    public float JoyMovementSpeed { get { return joyMovementSpeed; } }

    public Color DefaultColor { get { return defaultColor; } }
    public Color AlertColor { get { return alertColor; } }
    public Color AngerColor { get { return angerColor; } }
    public Color FearColor { get { return fearColor; } }
    public Color SadnessColor { get { return sadnessColor; } }
    public Color JoyColor { get { return joyColor; } }

    public float DefaultVisionConeRadius { get { return defaultVisionConeRadius; } }
    public float AlertVisionConeRadius { get { return alertVisionConeRadius; } }
    public float AngerVisionConeRadius { get { return angerVisionConeRadius; } }
    public float FearVisionConeRadius { get { return fearVisionConeRadius; } }
    public float SadnessVisionConeRadius { get { return sadnessVisionConeRadius; } }
    public float JoyVisionConeRadius { get { return joyVisionConeRadius; } }

    public float DefaultVisionConeViewAngle { get { return defaultVisionConeViewAngle; } }
    public float AlertVisionConeViewAngle { get { return alertVisionConeViewAngle; } }
    public float AngerVisionConeViewAngle { get { return angerVisionConeViewAngle; } }
    public float FearVisionConeViewAngle { get { return fearVisionConeViewAngle; } }
    public float SadnessVisionConeViewAngle { get { return sadnessVisionConeViewAngle; } }
    public float JoyVisionConeViewAngle { get { return joyVisionConeViewAngle; } } 

    public float PreAlertDuration { get { return preAlertDuration; } }
}
