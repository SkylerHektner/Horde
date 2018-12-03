﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeuristicCosts", menuName = "HeuristicCosts")]
public class HeuristicCosts : ScriptableObject
{
	[Header("Rage")]
	[SerializeField]
	private int attackCost;

	[SerializeField]
	private int explodeCost;

	[Header("Devotion")]
	[SerializeField]
	private int moveCost;

	[SerializeField]
	private int beckonCost;
	
	[Header("Joy")]
	[SerializeField]
	private int hugCost;

    [SerializeField]
    private int pickupCost;

    [Header("Fear")]
    [SerializeField]
    private int waitCost;

    [SerializeField]
    private int screamCost;


    public ResourceManager.ResourceType GetEmotion(HInterface.HType h)
    {
        switch (h)
        {
            case HInterface.HType.Attack:
            case HInterface.HType.Explode:
                return ResourceManager.ResourceType.Rage;
            case HInterface.HType.Beckon:
            case HInterface.HType.Move:
                return ResourceManager.ResourceType.Devotion;
            case HInterface.HType.Wait:
            case HInterface.HType.Scream:
                return ResourceManager.ResourceType.Fear;
            case HInterface.HType.Hug:                                  
            case HInterface.HType.Pickup:                                   
                return ResourceManager.ResourceType.Joy;
        }                                                                   
        return 0;
    }

    public float GetCost(HInterface.HType h)
    {
        switch (h)
        {
            case HInterface.HType.Attack:
                return attackCost;
            case HInterface.HType.Explode:
                return explodeCost;
            case HInterface.HType.Move:
                return moveCost;
            case HInterface.HType.Beckon:
                return beckonCost;
            case HInterface.HType.Pickup:
                return pickupCost;
            case HInterface.HType.Wait:
                return waitCost;
            case HInterface.HType.Hug:
                return hugCost;
            case HInterface.HType.Scream:
                return screamCost;
        }
        return -1;
    }

    public void SpendEmotion(HInterface.HType h)
	{
		switch(GetEmotion(h))
		{
			case ResourceManager.ResourceType.Fear:
				ResourceManager.Instance.SpendFear(GetCost(h));
				break;
            case ResourceManager.ResourceType.Joy:
                ResourceManager.Instance.SpendJoy(GetCost(h));
                break;
            case ResourceManager.ResourceType.Devotion:
                ResourceManager.Instance.SpendDevotion(GetCost(h));
                break;
            case ResourceManager.ResourceType.Rage:
                ResourceManager.Instance.SpendRage(GetCost(h));
                break;
        }
	}
}
