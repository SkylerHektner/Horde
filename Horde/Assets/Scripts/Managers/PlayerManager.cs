using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
	public static PlayerManager instance; // Singleton instance

    public GameObject Player { get; private set; }
    public GameObject SpellPrefab;
	private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        Player = GameObject.Find("Player");
    }

    private void Start()
    {
        //TestHeuristicCosts();
    }
}
