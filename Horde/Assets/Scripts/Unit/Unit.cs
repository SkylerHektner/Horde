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
    [SerializeField]
    private Image healthBarMask;

    // --Shared Variables-- //
    public string Description { get; set; }
    public int MaxHealth { get; set; }
    public int AttackDamage { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
    public float MovementSpeed { get; set; }
    public float DetectionRange { get; set; }
    public string UnitType { get; set; } // Probably don't need this but don't want to remove it right now because of compile errors.
    // --End of Shared Variables-- //

    // --Scriptable Objects-- //
    [SerializeField]
    private StatBlock statBlock;

    [SerializeField] 
    private Attack attack;
    // --End of Scriptable Objects-- //

    [SerializeField]
	private bool isPatrolling; // Set to true if this enemy should be on a patrol path.
    public bool IsPatrolling { get { return isPatrolling; } }

    [SerializeField]
	private Transform[] patrolPoints;
    public Transform[] PatrolPoints { get { return patrolPoints; } }

    private bool isMindControlled;
    public bool IsMindControlled 
    { 
        get { return isMindControlled; }
        set 
        { 
            isMindControlled = value;
            if(!isMindControlled)
                if(isPatrolling)
                    unitController.MoveToNextPatrolPoint();
        } 
    }

    [Header("Visible variables for debugging purposes.")]
    [SerializeField] // Serialized for debugging purposes.
    private int currentHealth;
    public int CurrentHealth 
    { 
        get {  return currentHealth; } 
        set 
        { 
            if(healthBarMask != null) { healthBarMask.fillAmount = (float)currentHealth / (float)MaxHealth; }
            currentHealth = value; 
        } 
    }

    [SerializeField] 
    private Unit currentTarget; // The enemy that the unit targets from a Target heuristic.
    public Unit CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }

    [SerializeField] 
    private Heuristic currentHeuristic;
    public Heuristic CurrentHeuristic { get { return currentHeuristic; } }

    private Vector3 initialPosition; 
    public Vector3 InitialPosition { get { return initialPosition; } } 
  
    private UnitController unitController;
    public UnitController UnitController { get { return unitController; } }

    private Transform projectileSpawn;
    public Transform ProjectileSpawn { get { return projectileSpawn; } }

    private int curHIndex = 0;
    private List<HInterface.HType> behaviors;
    private ResourceManager resourceManager;
    private Vector3 lastPos;
    private Animator anim;

    public bool beingCarried = false;
    
    private void Awake()
    {
        // Initialize the shared variables.
        if(statBlock == null)
            Debug.LogError("Need to set the stat block!");
        else
            statBlock.Initialize(this); // Initialize all of the unit stats.

        if(attack == null)
            Debug.LogError("Need to set the attack!");
        else
            attack.Initialize(this); // Initialize all of the attack values.

        if(isPatrolling)
            if(patrolPoints.Length == 0)
                Debug.LogWarning("Unit set to patrol but no patrol points have been set.");
    }

    private void Start()
    {
        currentHealth = MaxHealth; // Start the unit with max health.
        isMindControlled = false; // Start with default behavior.
        initialPosition = transform.position; // Set their initial position to where they start on the level.
        
        behaviors = new List<HInterface.HType>();
        unitController = GetComponent<UnitController>();
        resourceManager = GameObject.Find("GameManagers").GetComponent<ResourceManager>();
        projectileSpawn = gameObject.transform.Find("ProjectileSpawn");

        if (behaviors.Count > 0)
        {
            currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
            currentHeuristic.Init();
        }

        lastPos = transform.position;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHeuristic != null)
        {
            currentHeuristic.Execute();
        }

        // make them face the way they are walking
        if ((lastPos.x != transform.position.x || lastPos.z != transform.position.z) && !beingCarried)
        {
            transform.forward = transform.position - lastPos;
            lastPos = transform.position;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
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
            isMindControlled = false;
            return;
        }
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }
    public void OnMouseDown()
    {
        if(HTargetingTool.Instance.GettingInput)
        {
            return;
        }


        if(ResourceManager.Instance.HasEmotion(RadialMenuUI.Instance.GetHeuristicChain()) == false)
        {
            return;
        }
        /*
        foreach (HInterface.HType h in RadialMenuUI.Instance.GetHeuristicChain())
        {
            ResourceManager.Instance.SpendEmotion(h);  
        }
        PlayerManager.instance.CastSpell(gameObject.transform);
        */
    }

    public void OverrideHeuristics(List<HInterface.HType> newBehaviorSet)
    {
        //Add if statement to check if unit is already mind controlled
        //Add if statement to check if a class is properly selected by the player
        if (newBehaviorSet != null)
        {
            behaviors = newBehaviorSet;
            curHIndex = 0;
            isMindControlled = true;
            StartAI();
        }
        //Add if statement to check if there are enough resources to add the behaviors
        //swap out the behaviors list with the player selected one.
        //if not enough resouces
        //warn the player
    }

    [ContextMenu("Start AI")]
    public void StartAI()
    {
        curHIndex = 0;
        currentHeuristic = (Heuristic)gameObject.AddComponent(HInterface.GetHeuristic(behaviors[curHIndex]));
        currentHeuristic.Init();
    }
}
