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

    public List<HInterface.HType> behaviors;
    private int curHIndex = 0;

    private Heuristic currentHeuristic;

    public int CurrentHealth;

    public Unit CurrentTarget; // The enemy that the player finds while 'Seeking'

    public void Start()
    {
        if (behaviors.Count > 0)
        {
            gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
            currentHeuristic = GetComponent<Heuristic>();
            currentHeuristic.Init();
        } 
    }

    public void Update()
    {
        currentHeuristic.Execute();
    }

    /// <summary>
    /// This method is called by the Heuristic when it resolves
    /// so the unit knows to switch to the next Heuristic
    /// It destroys the current hueristic and swaps it to the next in line
    /// If we are at the end of the line, it goes back to the beginning of the list
    /// </summary>
    public void HResolved()
    {
        Destroy(currentHeuristic);
        curHIndex++;
        if (curHIndex >= behaviors.Count)
        {
            curHIndex = 0;
        }
        gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic = GetComponent<Heuristic>();
        currentHeuristic.Init();
    }
}
