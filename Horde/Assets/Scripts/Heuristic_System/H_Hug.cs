using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class H_Hug : Heuristic
{
	private float hugLength = 5f;  // How long should he hug for?
	private bool hugFlag = true;

	private NavMeshAgent agent;
    private bool moving = false;

	private SpriteRenderer heart;

	public override void Init()
	{
		base.Init();

		heart = GetComponentInChildren<SpriteRenderer>();

		agent = GetComponent<NavMeshAgent>();
        HTargetingTool.Instance.GetTarget(unit, TargetReady, "Select who to share your love with.");
	}

	public override void Execute()
	{
		if(unit.CurrentTarget == null)
            return;

		unit.UnitController.MoveTo(unit.CurrentTarget.transform.position);

        float distanceFromTarget = Vector3.Distance(transform.position, unit.CurrentTarget.transform.position);

		if(distanceFromTarget <= 1.2f)
		{
			unit.CurrentTarget.UnitController.IsMindControlled = true;
			unit.CurrentTarget.UnitController.StopMoving(); // Stop the target from moving.
			unit.UnitController.StopMoving(); // Stop itself from moving.

			if(hugFlag)
				StartCoroutine(HugTarget());
		}
	} 

	public override void Resolve()
	{
		base.Resolve();
	}

	private void TargetReady(Unit u)
	{
		unit.CurrentTarget = u;
		unit.UnitController.MoveTo(u.transform.position);
	}

	private IEnumerator HugTarget()
    {
		hugFlag = false;

		ShowHeart();

        yield return new WaitForSeconds(5.0f);

		HideHeart();

		unit.CurrentTarget.UnitController.IsMindControlled = false;
        Resolve();
    }

	private void ShowHeart()
	{
		Color tmp = heart.color;
 		tmp.a = 100f;
 		heart.color = tmp;
	}

	private void HideHeart()
	{
		Color tmp = heart.color;
 		tmp.a = 0f;
 		heart.color = tmp;
	}
}
