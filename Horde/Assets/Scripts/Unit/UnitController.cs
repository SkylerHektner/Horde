using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible of executing that main commands of the unit.
/// The Unit class will call most of these functions.
/// </summary>
public class UnitController : MonoBehaviour 
{
    [SerializeField]
    private StatBlock statBlock;

    [SerializeField] 
    private Attack attack;

    private Unit u;

    private void Start()
    {
        u = GetComponent<Unit>();

        if(statBlock == null)
            Debug.LogError("Set the stat block in the Unit Controller!");
        else
            statBlock.Initialize(u); // Initialize all of the unit stats.

        if(attack == null)
            Debug.LogError("Set the Attack in the Unit Controller!");
        else
            attack.Initialize(u); // Initialize all of the attack values.

            u.CurrentHealth = u.MaxHealth; // Start the unit with max health.
    }

    public void Attack()
    {
        attack.ExecuteAttack(u);
    }

    public void Move()
    {

    }

    public void Special()
    {

    }

    /// <summary>
    /// Subtracts health from the unit. Calls destroy is health drops to
    /// zero or below. Returns true if this instance of damage killed the unit
    /// </summary>
    public bool TakeDamage(int dmgAmount)
    {
        u.CurrentHealth -= dmgAmount;
        // TODO: Call a destroy function if health drops below 0.
        if (u.CurrentHealth <= 0)
        {
            gameObject.transform.SetParent(null); // this is important for UnitManager.UpdateUnits();
            Destroy(gameObject);
            UnitManager.instance.UpdateUnits();
        }

        return u.CurrentHealth <= 0;
    }
    public bool HealDamage(int dmgAmount)
    {
        if (u.CurrentHealth <= u.MaxHealth)
            u.CurrentHealth += dmgAmount;
 
        return u.CurrentHealth == u.MaxHealth;
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
