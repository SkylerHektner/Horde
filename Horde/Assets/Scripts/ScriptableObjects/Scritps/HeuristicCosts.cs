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

	public void SpendEmotion(HInterface.HType h)
	{
		switch(h)
		{
			case HInterface.HType.Attack:
				ResourceManager.Instance.SpendRage(attackCost);
				break;
			case HInterface.HType.Move:
				ResourceManager.Instance.SpendDevotion(moveCost);
				break;
			//case HInterface.HType.Seduce:
			//	ResourceManager.Instance.SpendDevotion(tauntCost);
			//	break;
			//case HInterface.HType.Pickup:
			//	ResourceManager.Instance.SpendDevotion(pickupCost);
			//	break;
			case HInterface.HType.Wait:
				ResourceManager.Instance.SpendTranquility(waitCost);
				break;
		}
	}
}
