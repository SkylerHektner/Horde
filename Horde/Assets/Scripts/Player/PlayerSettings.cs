using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Settings", fileName = "PlayerData")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dartDuration;
    [SerializeField] private int costPerShot;
    [SerializeField] private int dartBounces;
    [SerializeField] private int maxJuice;

    public float MovementSpeed { get { return movementSpeed; } }
    public float DartDuration { get { return dartDuration; } }
    public int CostPerShot { get { return costPerShot; } }
    public int DartBounces { get { return dartBounces; } }
    public int MaxJuice { get { return maxJuice; } }
}
