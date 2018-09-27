using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves as a hub for other classes to get information
/// about other units in the game, ally or enemy
/// all unit classes are responsible for registering with it on awake
/// </summary>
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance; // Singleton instance

    private Unit[] enemies;
    private Unit[] allies;
    private GameObject enemyContainer;
    private GameObject allyContainer;

    /// <summary>
    /// returns the number of living enemies
    /// </summary>
    public int EnemyCount { get { return enemies.Length; } }

    /// <summary>
    /// returns the number of living allies
    /// </summary>
    public int AllyCount { get { return allies.Length; } }

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start ()
    {
        // Store the enemies for distance calculations
        enemyContainer = GameObject.Find("Enemies");
        enemies = enemyContainer.GetComponentsInChildren<Unit>();
        allyContainer = GameObject.Find("Allies");
        allies = allyContainer.GetComponentsInChildren<Unit>();
    }

    public void StartAllyAI()
    {
        foreach(Unit u in allies)
        {
            u.StartAI();
        }
    }

    /// <summary>
    /// Returns an enemy closest to the given position.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestEnemy(Vector3 unitPosition)
    {
        UpdateUnits();
        return FindClosestUnit(enemies, unitPosition);
    }

    /// <summary>
    /// Returns an ally closest to the given position.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestAlly(Vector3 unitPosition)
    {
        UpdateUnits();
        return FindClosestUnit(allies, unitPosition);
    }



    public Unit GetClosestRangedEnemy(Vector3 unitPosition)
    {
        UpdateUnits();
        List<Unit> rangedEnemies = new List<Unit>();
        foreach(Unit x in enemies)
        {
            if(x.unitType == "ranged")
            {
                rangedEnemies.Add(x);
            }
        }
        if (rangedEnemies.Count > 0)
        {
            return FindClosestUnit(rangedEnemies.ToArray(), unitPosition);
        }
        else
        {
            return FindClosestUnit(enemies, unitPosition);
        }
    }

    public Unit GetClosestMeleeEnemy(Vector3 unitPosition)
    {
        UpdateUnits();
        List<Unit> meleeEnemies = new List<Unit>();
        foreach (Unit x in enemies)
        {
            if (x.unitType == "melee")
            {
                meleeEnemies.Add(x);
            }
        }
        if (meleeEnemies.Count > 0)
        {
            return FindClosestUnit(meleeEnemies.ToArray(), unitPosition);
        }
        else
        {
            return FindClosestUnit(enemies, unitPosition);
        }
    }

    public Unit GetWeakestEnemy(Vector3 unitPosition)
    {
        UpdateUnits();
        Unit lowHPUnit = enemies[0];
        for(int x = 1; x < enemies.Length; x++)
        {
            if(lowHPUnit.currentHealth > enemies[x].currentHealth)
            {
                lowHPUnit = enemies[x];
            }
        }
        return lowHPUnit;
    }

    public Unit GetWeakestAlly(Vector3 unitPosition, Unit exludeUnit = null)
    {
        UpdateUnits();
        Unit lowHPUnit = allies[0];
        for (int x = 1; x < allies.Length; x++)
        {
            if (lowHPUnit.currentHealth > allies[x].currentHealth)
            {
                if (exludeUnit != null && allies[x] != exludeUnit)
                {
                    lowHPUnit = allies[x];
                }
                else
                {
                    lowHPUnit = allies[x];
                }
            }
        }
        return lowHPUnit;
    }

        /// <summary>
        /// given a set of units and a position, finds the closest unit from the set
        /// to that position
        /// </summary>
        /// <param name="units"></param>
        /// <param name="unitPosition"></param>
        /// <returns></returns>
        private Unit FindClosestUnit(Unit[] units, Vector3 unitPosition)
    {
        if (units.Length == 0)
            return null;

        float closestDistance = 10000f;
        Unit closestUnit = units[0];

        foreach (Unit unit in units)
        {
            if (unit == null) // Hacky patch to remove error.
                continue;

            float distance = Vector3.Distance(unit.transform.position, unitPosition);
            if (distance <= closestDistance)
            {
                if (distance == 0)
                {
                    //if the unit targets itself it doesn't count
                }
                else
                {
                    closestDistance = distance;
                    closestUnit = unit;
                }
            }
        }

        return closestUnit;
    }

    /// <summary>
    /// Updates the internal data structure that holds the enemies or allies.
    /// We might need to do this because an enemy or ally may have been destroyed.
    /// </summary>
    public void UpdateUnits(Unit unitToRemove = null)
    {
        enemies = enemyContainer.GetComponentsInChildren<Unit>();
        allies = allyContainer.GetComponentsInChildren<Unit>();
    }
}
