using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
	public static PlayerManager instance; // Singleton instance

    public GameObject Player { get; private set; }

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

    private void TestHeuristicCosts()
    {
        Debug.Log(ResourceManager.Instance.Rage);
        ResourceManager.Instance.SpendEmotion(HInterface.HType.Attack);
        Debug.Log(ResourceManager.Instance.Rage);
    }
}
