using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (VisionCone))]
public class VisionConeEditor : Editor
{
	void OnSceneGUI()
	{
		VisionCone vc = (VisionCone)target;

		Handles.color = Color.white;
		Handles.DrawWireArc(vc.transform.position, Vector3.up, Vector3.forward, 360, vc.ViewRadius);

		Vector3 viewAngleA = vc.DirFromAngle(-vc.ViewAngle / 2, false);
		Vector3 viewAngleB = vc.DirFromAngle(vc.ViewAngle / 2, false);

		Handles.DrawLine(vc.transform.position, vc.transform.position + viewAngleA * vc.ViewRadius);
		Handles.DrawLine(vc.transform.position, vc.transform.position + viewAngleB * vc.ViewRadius);

		// Draw lines from the enemy to the visible target.
		Handles.color = Color.red;
		foreach(Transform visibleTarget in vc.VisibleTargets)
		{
			Handles.DrawLine(vc.transform.position, visibleTarget.position);
		}
	}
}
