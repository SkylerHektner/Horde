using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour 
{
	public float ViewRadius { get { return viewRadius; } }
	public float ViewAngle { get { return viewAngle; } }
	public List<Transform> VisibleTargets { get { return visibleTargets; } }

	[SerializeField] private float viewRadius;
	[SerializeField, Range(0, 360)] private float viewAngle;
	[SerializeField] private LayerMask targetMask;
	[SerializeField] private LayerMask obstacleMask;
	
	private List<Transform> visibleTargets = new List<Transform>();

	private void Start()
	{
		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	/// <summary>
	/// Takes in an angle and spits out the direction of that angle.
	/// </summary>
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
			angleInDegrees += transform.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	private IEnumerator FindTargetsWithDelay(float delay)
	{
		while(true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	private void FindVisibleTargets()
	{
		visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for(int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			
			// Check if the target is within the view angle
			if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float distanceToTarget = Vector3.Distance(transform.position, target.position);

				if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)) // No obstacles are in the way
				{
					visibleTargets.Add(target);
				}
			}
		}
	}
}
