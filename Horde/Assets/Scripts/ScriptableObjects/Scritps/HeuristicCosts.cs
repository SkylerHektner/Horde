using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeuristicCosts", menuName = "HeuristicCosts")]
public class HeuristicCosts : ScriptableObject
{
	[Header("Rage")]
	[SerializeField]
	private int attackCost;

	[Header("Devotion")]
	[SerializeField]
	private int moveCost;

	[SerializeField]
	private int tauntCost;

	[SerializeField]
	private int pickupCost;
	
	[Header("Tranquility")]
	[SerializeField]
	private int waitCost;

	public void SpendEmotion(Behavior h)
	{
		switch(h)
		{
			case Behavior.Attack:
				ResourceManager.Instance.SpendRage(attackCost);
				break;
			case Behavior.Move:
				ResourceManager.Instance.SpendDevotion(moveCost);
				break;
			case Behavior.Taunt:
				ResourceManager.Instance.SpendDevotion(tauntCost);
				break;
			case Behavior.Pickup:
				ResourceManager.Instance.SpendDevotion(pickupCost);
				break;
			case Behavior.Wait:
				ResourceManager.Instance.SpendTranquility(waitCost);
				break;
		}
	}
}

public enum Behavior
{
	Attack,
	Move,
	Taunt,
	Pickup,
	Wait
}
