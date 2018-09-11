using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for all units in the game. It is responsible for managing Heuristics.
/// It also exposes information that Heuristics might be interested in like health, current target, etc...
/// </summary>
public class Unit : MonoBehaviour
{
    [SerializeField]
    private bool useHeuristicSwapping = true;

    private List<Heuristic> behaviors = new List<Heuristic>();

    public int CurrentHealth;

    public Unit CurrentTarget;
}
