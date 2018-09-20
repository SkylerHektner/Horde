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
    public Unit CalculateClosestEnemy(Vector3 unitPosition)
    {
        return findClosestUnit(enemies, unitPosition);
    }

    /// <summary>
    /// Returns an ally closest to the given position.
    /// </summary>
    /// <returns></returns>
    public Unit CalculateClosestAlly(Vector3 unitPosition)
    {
        return findClosestUnit(allies, unitPosition);
    }

    /// <summary>
    /// given a set of units and a position, finds the closest unit from the set
    /// to that position
    /// </summary>
    /// <param name="units"></param>
    /// <param name="unitPosition"></param>
    /// <returns></returns>
    private Unit findClosestUnit(Unit[] units, Vector3 unitPosition)
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
                closestDistance = distance;
                closestUnit = unit;
            }
        }

        return closestUnit;
    }

    /// <summary>
    /// Updates the internal data structure that hold the enemies or allies.
    /// We might need to do this because an enemy or ally may have been destroyed.
    /// </summary>
    public void UpdateUnits()
    {
        enemies = enemyContainer.GetComponentsInChildren<Unit>();
        allies = allyContainer.GetComponentsInChildren<Unit>();
    }
}
