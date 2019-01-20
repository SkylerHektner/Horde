using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathosUI : MonoBehaviour 
{
	public static PathosUI instance;

	public ResourceManager.ResourceType CurrentEmotion { get { return currentEmotion; } set { currentEmotion = value; } }

	private ResourceManager.ResourceType currentEmotion;

	private void Awake() 
	{
		if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
	}
	
	void Update () 
	{
		
	}
}
