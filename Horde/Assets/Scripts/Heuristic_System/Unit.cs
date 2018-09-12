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

    private Heuristic currentHeuristic;

    public int CurrentHealth;

    public Unit CurrentTarget; // The enemy that the player finds while 'Seeking'

    public void Start()
    {
        // TEMPORARY way of adding a heuristic to the unit(Should be added via the UI)
        Heuristic seekBehavior = gameObject.AddComponent<H_Seek>();
        seekBehavior.Init();
        currentHeuristic = seekBehavior;
        
        behaviors.Add(seekBehavior); 
    }

    public void Update()
    {
        currentHeuristic.Execute();
    }

    void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Enemy")
            currentHeuristic.Resolve();
    }
}
