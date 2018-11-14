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
    [Header("UI Stuff:")]
    [SerializeField]
    private Image healthBarMask;

    // ---- //

    [Header("AI Stuff:")]
    [SerializeField]
    private bool useHeuristicSwapping = true;

    [SerializeField] 
    private bool startAIImmediate = false; // if true, starts AI on simulation start

    [SerializeField]
    private Transform projectileSpawn;
    public Transform ProjectileSpawn { get { return projectileSpawn; } }

    [SerializeField] public List<HInterface.HType> behaviors;
    public List<HInterface.HType> Behaviors { get { return behaviors; } }

    // --Shared Variables-- //
    public int MaxHealth { get; set; }
    public int AttackDamage { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
    public string UnitType { get; set; }
    public float MovementSpeed { get; set; }
    public float DetectionRange { get; set; }
    private ResourceManager resourceManager;

    // --Non-Shared Variables-- //
    [Header("For debugging. Don't change these in the editor")]
    [SerializeField] 
    private int currentHealth;
    public int CurrentHealth 
    { 
        get {  return currentHealth; } 
        set 
        { 
            if(healthBarMask != null) { healthBarMask.fillAmount = (float)CurrentHealth / (float)MaxHealth; }
            currentHealth = value; 
        } 
    }

    [SerializeField] 
    private Unit currentTarget; // The enemy that the unit targets from a Target heuristic.
    public Unit CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }

    [SerializeField] 
    private Heuristic currentHeuristic;
    public Heuristic CurrentHeuristic { get { return currentHeuristic; } }

    private UnitController unitController;
    public UnitController UnitController { get { return unitController; } }

    private int curHIndex = 0;
    
    public void Awake()
    {
        GameObject managers = GameObject.Find("Game Managers");
        resourceManager = managers.GetComponent<ResourceManager>();
        unitController = GetComponent<UnitController>();
        unitController.InitializeController();

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
    public void OnMouseDown()
    {
         //Add if statement to check if unit is already mind controlled
         //Add if statement to check if a class is properly selected by the player
         //Add if statement to check if there are enough resources to add the behaviors
            //swap out the behaviors list with the player selected one.
        //if not enough resouces
            //warn the player
    }
    [ContextMenu("Start AI")]
    public void StartAI()
    {
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }
}
