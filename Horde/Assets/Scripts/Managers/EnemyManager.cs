using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour 
{
	public static EnemyManager instance;

	public Player targetedPlayer;

	private Enemy[] enemies;
	private bool enemiesAlerted;

	private void Awake() 
	{
		if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
	}

	private void Start()
	{
		enemies = FindObjectsOfType<Enemy>();
	}
	
	private void Update() 
	{
		if(enemiesAlerted)
		{
			foreach(Enemy e in enemies)
			{
				if(e.GetComponent<VisionCone>().TryGetPlayer())
				{
					return;
				}
			}
		}
	}

	/// <summary>
	///	Changes all the enemies in the room to the alert state
	/// if they have a valid path to the player.
	/// </summary>
	public void AlertEnemies()
	{
		Enemy[] enemies = FindObjectsOfType<Enemy>();

		if(enemies.Length != 0)
		{
			foreach(Enemy e in enemies)
			{
				// TODO: Add check here to see if enemy has valid path to player.
				if(e == null)
					return;
					
				e.ChangeState(new Alert(e.GetComponent<Enemy>()));
			}
		}
	}
}
