﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class serves as a hub for other classes to get information
/// about other units in the game, ally or enemy
/// all unit classes are responsible for registering with it on awake
/// 
/// EVERY UNIT IS RESPONSIBLE FOR HAVING A TEAM ONE OR TEAM TWO TAG
/// </summary>
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance; // Singleton instance

    private Unit[] teamOneUnits; // The units that the player creates.
    private Unit[] teamTwoUnits; // The units that are built into each level. Enemies of the player units.

    private GameObject teamOneUnitContainer;
    private GameObject teamTwoUnitContainer;

    public class LevelEndEvent : UnityEvent<bool> { }
    public LevelEndEvent LevelEnd;

    /// <summary>
    /// Returns the number of living units from Team One
    /// </summary>
    public int TeamOneUnitCount { get { return teamOneUnits.Length; } }

    /// <summary>
    /// Returns the number of living units from Team Two
    /// </summary>
    public int TeamTwoUnitCount { get { return teamTwoUnits.Length; } }

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
        teamOneUnitContainer = GameObject.Find("TeamOne");
        teamOneUnits = teamOneUnitContainer.GetComponentsInChildren<Unit>();
        teamTwoUnitContainer = GameObject.Find("TeamTwo");
        teamTwoUnits = teamTwoUnitContainer.GetComponentsInChildren<Unit>();
    }

    public void StartTeamOneAI()
    {
        foreach(Unit u in teamOneUnits)
        {
            u.StartAI();
        }
    }

    /// <summary>
    /// Returns the closest enemy to the given unit.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestEnemy(Unit u)
    {
        UpdateUnits();

        Unit[] units = GetEnemyUnits(u);
        return FindClosestUnit(units, u.transform.position);
    }

    /// <summary>
    /// Returns the enemy with the lowest amount of health.
    /// </summary>
    /// <returns></returns>
    public Unit GetWeakestEnemy(Unit u)
    {
        UpdateUnits();

        Unit[] units = GetEnemyUnits(u);

        Unit lowHPUnit = units[0];
        for (int x = 1; x < units.Length; x++)
        {
            if (lowHPUnit.CurrentHealth > units[x].CurrentHealth)
            {
                lowHPUnit = units[x];
            }
        }
        return lowHPUnit;
    }

    /// <summary>
    /// Returns the closest ranged enemy to the given unit.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestRangedEnemy(Unit u)
    {
        UpdateUnits();

        Unit[] units = GetEnemyUnits(u);

        List<Unit> rangedEnemies = new List<Unit>();
        foreach(Unit x in units)
        {
            if(x.UnitType == "Ranged")
            {
                rangedEnemies.Add(x);
            }
        }

        if (rangedEnemies.Count > 0)
        {
            return FindClosestUnit(rangedEnemies.ToArray(), u.transform.position);
        }
        else
        {
            return FindClosestUnit(units, u.transform.position); // If there aren't any ranged enemies, find any closest enemy.
        }
    }

    /// <summary>
    /// Returns the closest ranged enemy to the given unit.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestMeleeEnemy(Unit u)
    {
        UpdateUnits();

        Unit[] units = GetEnemyUnits(u);

        List<Unit> rangedEnemies = new List<Unit>();
        foreach (Unit x in units)
        {
            if (x.UnitType == "Ranged")
            {
                rangedEnemies.Add(x);
            }
        }

        if (rangedEnemies.Count > 0)
        {
            return FindClosestUnit(rangedEnemies.ToArray(), u.transform.position);
        }
        else
        {
            return FindClosestUnit(units, u.transform.position); // If there aren't any melee enemies, find any closest enemy.
        }
    }

    /// <summary>
    /// Returns the closest ally to the given unit.
    /// </summary>
    /// <returns></returns>
    public Unit GetClosestAlly(Unit u)
    {
        UpdateUnits();

        Unit[] units = GetAlliedUnits(u);
        return FindClosestUnit(units, u.transform.position);
    }

    /// <summary>
    /// Returns the ally with the lowest amount of health.
    /// </summary>
    /// <returns></returns>
    public Unit GetWeakestAlly(Unit u, Unit exludeUnit = null)
    {
        UpdateUnits();

        Unit[] units = GetAlliedUnits(u);

        Unit lowHPUnit = units[0];
        for (int x = 1; x < units.Length; x++)
        {
            if (lowHPUnit.CurrentHealth > units[x].CurrentHealth)
            {
                if (exludeUnit != null && units[x] != exludeUnit)
                {
                    lowHPUnit = units[x];
                }
                else
                {
                    lowHPUnit = units[x];
                }
            }
        }
        return lowHPUnit;
    }

    /// <summary>
    /// Given a set of units and a position, finds the closest unit from the set
    /// to that position
    /// </summary>
    /// <returns></returns>
    private Unit FindClosestUnit(Unit[] units, Vector3 unitPosition)
    {
        // TODO: This function needs to calculate the distance of the travel distance through the nav mesh.
        //       Right now it calculates the raw Vector3 distance between two units.

        if (units.Length == 0) // Don't bother if there aren't any units to search through.
            return null;

        float closestDistance = 10000f;
        Unit closestUnit = units[0];

        foreach (Unit unit in units)
        {
            float distance = Vector3.Distance(unit.transform.position, unitPosition);
            if (distance <= closestDistance)
            {
                if (distance == 0)
                {
                    //if the unit targets itself it doesn't count
                    continue;
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
    /// Returns the array of units that are the allies of the passed in unit.
    /// </summary>
    /// <param name="u"></param>
    private Unit[] GetAlliedUnits(Unit u)
    {
        if (u.gameObject.tag == "TeamOneUnit")
            return teamOneUnits;
        else if (u.gameObject.tag == "TeamTwoUnit")
            return teamTwoUnits;
        else
        {
            Debug.LogError("Unit is not tagged with a team.");
            return null;
        }
    }

    /// <summary>
    /// Returns the array of units that are the enemies of the passed in unit.
    /// </summary>
    /// <param name="u"></param>
    private Unit[] GetEnemyUnits(Unit u)
    {
        if (u.gameObject.tag == "TeamOneUnit")
            return teamTwoUnits;
        else if (u.gameObject.tag == "TeamTwoUnit")
            return teamOneUnits;
        else
        {
            Debug.LogError("Unit is not tagged with a team.");
            return null;
        }
    }

    /// <summary>
    /// Updates the internal data structure that holds the enemies or allies.
    /// We need to do this because an enemy or ally may have been destroyed.
    /// 
    /// If either teams count is below 0 then it invokes the LevelEndEvent
    /// a true value means team one units are all dead
    /// a false value means team two units are all dead
    /// </summary>
    public void UpdateUnits(Unit unitToRemove = null)
    {
        teamTwoUnits = teamTwoUnitContainer.GetComponentsInChildren<Unit>();
        teamOneUnits = teamOneUnitContainer.GetComponentsInChildren<Unit>();

        if(TeamOneUnitCount <= 0)
        {
            LevelEnd.Invoke(true);
        }
        else if (TeamTwoUnitCount <= 0)
        {
            LevelEnd.Invoke(false);
        }
    }
}
