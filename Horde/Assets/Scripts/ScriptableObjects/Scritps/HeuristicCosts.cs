using System.Collections;
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
	private int pickupCost;

	[SerializeField]
	private int beckonCost;
	
	[Header("Tranquility")]
	[SerializeField]
	private int waitCost;

	[SerializeField]
	private int hugCost;

	public void SpendEmotion(HInterface.HType h)
	{
		switch(h)
		{
			case HInterface.HType.Attack:
				ResourceManager.Instance.SpendRage(attackCost);
				break;
			case HInterface.HType.Explode:
				ResourceManager.Instance.SpendRage(explodeCost);
				break;
			case HInterface.HType.Move:
				ResourceManager.Instance.SpendDevotion(moveCost);
				break;
			case HInterface.HType.Beckon:
				ResourceManager.Instance.SpendDevotion(beckonCost);
				break;
			case HInterface.HType.Pickup:
				ResourceManager.Instance.SpendDevotion(pickupCost);
				break;
			case HInterface.HType.Wait:
				ResourceManager.Instance.SpendTranquility(waitCost);
				break;
			case HInterface.HType.Hug:
				ResourceManager.Instance.SpendTranquility(hugCost);
				break;
		}
	}
}
