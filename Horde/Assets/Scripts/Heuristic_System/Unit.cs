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

    [SerializeField]
    private List<HInterface.HType> behaviors;

    private Heuristic currentHeuristic;

    public int CurrentHealth;

    public Unit CurrentTarget; // The enemy that the player finds while 'Seeking'

    public void Start()
    {
        if (behaviors.Count > 0)
        {
            gameObject.AddComponent(HInterface.GetHeuristic(behaviors[0]));
            currentHeuristic = GetComponent<Heuristic>();
            currentHeuristic.Init();
        } 
    }

    public void Update()
    {
        currentHeuristic.Execute();
    }
}
