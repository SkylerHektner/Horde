using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the base class for all units in the game. It is responsible for managing Heuristics,
/// and reading / storing data from data containers.
/// </summary>
public class Unit : MonoBehaviour
{
    [Header("Unit Stats:")]
    [SerializeField]
    private StatBlock statBlock;

    [SerializeField] 
    private Attack attack;
    public Attack Attack { get { return attack; } }

    // ---- //
    
    [Header("UI Stuff:")]
    [SerializeField]
    private Image healthBarMask;

    // ---- //

    [Header("AI Stuff:")]
    [SerializeField]
    private bool useHeuristicSwapping = true;

    [SerializeField] 
    private bool startAIImmediate = false; // if true, starts AI on simulation start

    [SerializeField] public List<HInterface.HType> behaviors;
    public List<HInterface.HType> Behaviors { get { return behaviors; } }

    // --Shared Variables-- //
    public int MaxHealth { get; set; }
    public int AttackDamage { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
    public string UnitType { get; set; }
    public float MovementSpeed { get; set; }
    public float TrajectoryAngle { get; set; }
    public Transform ProjectilePrefab { get; set; }
    public Transform ParticleEffectPrefab { get; set; }

    // --Non-Shared Variables-- //
    [Header("For debugging. Don't change these in the editor")]
    [SerializeField] private int currentHealth;
    public int CurrentHealth { get; set; }
    [SerializeField] private Unit currentTarget; // The enemy that the unit targets from a Target heuristic.
    public Unit CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }
    [SerializeField] private Heuristic currentHeuristic;
    public Heuristic CurrentHeuristic { get { return currentHeuristic; } }

    private int curHIndex = 0;
    

    public Transform projectileSpawn;

    public void Start()
    {
        statBlock.Initialize(gameObject); // Initialize all of the unit stats.
        attack.Initialize(gameObject); // Initialize all of the attack values.
        
        CurrentHealth = MaxHealth; // Start the unit with max health.

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
        CurrentHealth -= dmgAmount;
        if (healthBarMask != null)
        {
            healthBarMask.fillAmount = (float)CurrentHealth / (float)MaxHealth;
        }
        // TODO: Call a destroy function if health drops below 0.
        if (CurrentHealth <= 0)
        {
            gameObject.transform.SetParent(null); // this is important for UnitManager.UpdateUnits();
            Destroy(gameObject);
            UnitManager.instance.UpdateUnits();
        }

        return CurrentHealth <= 0;
    }
    public bool HealDamage(int dmgAmount)
    {
        if (CurrentHealth <= MaxHealth)
        {
            CurrentHealth += dmgAmount;
        }       
        if (healthBarMask != null)
        {
            healthBarMask.fillAmount = (float)CurrentHealth / (float)MaxHealth;
        }
 
        return CurrentHealth == MaxHealth;
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
            Projectile p = collision.gameObject.GetComponent<Projectile>();

            if(p.team == Team.TeamTwo)
            {
                Destroy(collision.gameObject);
                TakeDamage(p.damage);
            }

            if(collision.gameObject.tag == "Heal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
        else if(gameObject.tag == "TeamTwoUnit") // Unit is on team two.
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();

            if (p.team == Team.TeamOne)
            {
                Destroy(collision.gameObject);
                TakeDamage(p.damage);
            }

            if (collision.gameObject.tag == "Heal")
            {
                Destroy(collision.gameObject);
                HealDamage(1);
            }
        }
    }
}
