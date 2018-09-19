using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance; // Singleton instance

    private Unit[] enemies;
    private GameObject enemyContainer;

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start ()
    {
        // Store the enemies for distance calculations
        enemyContainer = GameObject.Find("Enemies");
        enemies = enemyContainer.GetComponentsInChildren<Unit>();
    }
	
	void Update ()
    {
		
	}

    /// <summary>
    /// Returns an enemy closest to the given position.
    /// </summary>
    /// <returns></returns>
    public Unit CalculateClosestEnemy(Vector3 unitPosition)
    {
        if (enemies.Length == 0)
            return null;

        float closestDistance = 10000f;
        Unit closestEnemy = enemies[0];

        foreach (Unit enemy in enemies)
        {
            if (enemy == null) // Hacky patch to remove error.
                continue;
            float distance = Vector3.Distance(enemy.transform.position, unitPosition);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// Returns the amount of enemies that are alive.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        return enemies.Length;
    }

    /// <summary>
    /// Updates the internal data structure that hold the enemies.
    /// We might need to do this because an enemy may have been destroyed.
    /// </summary>
    public void UpdateEnemies()
    {
        enemies = enemyContainer.GetComponentsInChildren<Unit>();
    }
}
