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
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private bool StartAIImmediate = false; // if true, starts AI on simulation start

    public Unit currentTarget; // The enemy that the player finds while 'Seeking'
    public List<HInterface.HType> behaviors;

    private int curHIndex = 0;
    private Heuristic currentHeuristic;

    public void Start()
    {
        if (behaviors.Count > 0 && StartAIImmediate)
        {
            currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
            currentHeuristic.Init();
        } 
    }

    private void Update()
    {
        if (currentHeuristic != null)
        {
            currentHeuristic.Execute();
        }
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
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }

    /// <summary>
    /// Subtracts health from the unit. Calls destroy is health drops to
    /// zero or below. Returns true if this instance of damage killed the unit
    /// </summary>
    public bool TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;

        // TODO: Call a destroy function if health drops below 0.
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        return currentHealth <= 0;
    }

    [ContextMenu("Start AI")]
    public void StartAI()
    {
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            if(gameObject.tag == "Enemy") // We only want projectiles to effect enemies.
            {
                Destroy(collision.gameObject);
                TakeDamage(1);
            }
        }
    }
}
