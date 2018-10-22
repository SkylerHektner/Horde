using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the base class for all units in the game. It is responsible for managing Heuristics.
/// It also exposes information that Heuristics might be interested in like health, current target, etc...
/// </summary>
public class Unit : MonoBehaviour
{
    [Header("Unit Stats:")]
    [SerializeField]
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    [SerializeField]
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    [SerializeField]
    private float attackVelocity;
    public float AttackVelocity { get { return attackVelocity; } }

    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }

    [SerializeField]
    private float attackCooldown;
    public float AttackCooldown { get { return attackCooldown; } }

    [SerializeField]
    private string unitType;
    public string UnitType { get { return unitType; } }
    
    
    [Header("UI Stuff:")]
    [SerializeField]
    private Image healthBarMask;

    [Header("AI Stuff:")]
    [SerializeField]
    private bool useHeuristicSwapping = true;
    [SerializeField]
    private bool startAIImmediate = false; // if true, starts AI on simulation start

    public Unit currentTarget; // The enemy that the player finds while 'Seeking'
    public List<HInterface.HType> behaviors;

    private int curHIndex = 0;
    private Heuristic currentHeuristic;

    public void Start()
    {
        if (behaviors.Count > 0 && startAIImmediate)
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
        if (healthBarMask != null)
        {
            healthBarMask.fillAmount = (float)currentHealth / (float)maxHealth;
        }
        // TODO: Call a destroy function if health drops below 0.
        if (currentHealth <= 0)
        {
            gameObject.transform.SetParent(null); // this is important for UnitManager.UpdateUnits();
            Destroy(gameObject);
            UnitManager.instance.UpdateUnits();
        }

        return currentHealth <= 0;
    }
    public bool HealDamage(int dmgAmount)
    {
        if (currentHealth <= maxHealth)
        {
            currentHealth += dmgAmount;
        }       
        if (healthBarMask != null)
        {
            healthBarMask.fillAmount = (float)currentHealth / (float)maxHealth;
        }
 
        return currentHealth == maxHealth;
    }

    [ContextMenu("Start AI")]
    public void StartAI()
    {
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check which team this unit is on.
        if(gameObject.tag == "TeamOneUnit") // Unit is on team one.
        {
            if(collision.gameObject.tag == "TeamTwoProjectile")
            {
                Destroy(collision.gameObject);
                TakeDamage(1);
            }

            if(collision.gameObject.tag == "TeamOneHeal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
        else if(gameObject.tag == "TeamTwoUnit") // Unit is on team two.
        {
            if (collision.gameObject.tag == "TeamOneProjectile")
            {
                Destroy(collision.gameObject);
                TakeDamage(1);
            }

            if (collision.gameObject.tag == "TeamOneHeal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }


        /*
        if(collision.gameObject.tag == "Projectile")
        {
            if(gameObject.tag == "TeamTwoUnit") // We only want projectiles to effect enemies.
            {
                Destroy(collision.gameObject);
                TakeDamage(1);
            }
        }

        if(collision.gameObject.tag == "EnemyProjectile")
        {
            if(gameObject.layer == 11) // If it's the ally layer
            {
                Destroy(collision.gameObject);
                TakeDamage(1);
            }
        }

        if(collision.gameObject.tag == "HealProjectile")
        {
            if(gameObject.layer == 11) // If it's the ally layer
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
        */
    }
}
