﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is heavily based on the youtube tutorial https://www.youtube.com/watch?v=rQG9aUWarwE&list=PLFt_AvWsXl0dohbtVgHDNmgZV_UY7xZv7

[RequireComponent(typeof(Enemy))]
public class VisionCone : MonoBehaviour 
{
	//public event Action OnTargetEnteredVision = delegate { };
	//public event Action OnTargetExitedVision = delegate { };

	public float ViewRadius { get { return viewRadius; } }
	public float ViewAngle { get { return viewAngle; } }
	public List<Transform> VisibleTargets { get { return visibleTargets; } }

	[SerializeField] private float viewRadius;
	[SerializeField, Range(0, 360)] private float viewAngle;
	//[SerializeField] private List<LayerMask> targetMasks;
	[SerializeField] private List<LayerMask> obstacleMasks;
	[SerializeField] private float meshResolution;
	[SerializeField] private MeshFilter viewMeshFilter;

	private  Mesh viewMesh;
	private Enemy enemy;
	private List<Transform> visibleTargets = new List<Transform>();
	private LayerMask targetMask;
	private LayerMask obstacleMask;
	private bool playerInVision = false;

	private void Start()
	{
		foreach(LayerMask mask in obstacleMasks)
		{
			obstacleMask = obstacleMask | mask; // Create one Layer Mask for obstacles.
		}

		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		enemy = GetComponent<Enemy>();
		StartCoroutine(FindTargetsWithDelay(0.05f));
	}

	private void Update()
	{
		if(targetMask == LayerMask.GetMask("Player")) // If the Layer Mask is for only the player.
		{
			if(!playerInVision && visibleTargets.Count == 1)
			{
				//OnTargetEnteredVision();
				playerInVision = true;
			}

			if(playerInVision && visibleTargets.Count == 0)
			{
				//OnTargetExitedVision();
				playerInVision = false;
			}
		}

		//Debug.Log(playerInVision);
	}

	private void LateUpdate() //Only gets called AFTER the controller is updated.
	{
		DrawVisionCone();
	}

	/// <summary>
	///	Loops through the visible targets to try to find the player.
	/// </summary>
	public Player TryGetPlayer()
	{
		foreach(Transform t in visibleTargets)
		{
			if(t.GetComponent<Player>() != null) 
			{
				return t.GetComponent<Player>();
			}
		}

		return null;
	}

	/// <summary>
	///	Changes the color of the vision cone.
	/// </summary>
	public void ChangeColor(Color c)
	{
		transform.GetComponentInChildren<MeshRenderer>().materials[0].color = c;
	}

	/// <summary>
	///	Changes which layers the vision cone considers targets.
	///
	/// Used for when an enemy changes behaviors and looks for
	/// targets other than the player.
	/// </summary>
	public void ChangeTargetMask(LayerMask layerMask)
	{
		targetMask = layerMask;
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

	private void DrawVisionCone()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;

		List<Vector3> viewPoints = new List<Vector3>();

        int vertexCount = stepCount + 2;
        Vector2[] UV = new Vector2[vertexCount];
        UV[0] = new Vector2(0, 0.5f);
        for (int i = 0; i <= stepCount; i++)
		{
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);
            UV[i+1] = new Vector2(newViewCast.dst / viewRadius, (float)i / (float)vertexCount);
            viewPoints.Add(newViewCast.point);
			//Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
		}

		
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;

		for(int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if(i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
        viewMesh.uv = UV;
		viewMesh.RecalculateNormals();
	}

	public ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		else
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
	}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
		{
			this.hit = hit;
			this.point = point;
			this.dst = dst;
			this.angle = angle;
		}
	}
}
