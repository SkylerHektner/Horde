using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This class is heavily based on the youtube tutorial https://www.youtube.com/watch?v=rQG9aUWarwE&list=PLFt_AvWsXl0dohbtVgHDNmgZV_UY7xZv7

public class VisionCone : MonoBehaviour 
{
	//public event Action OnTargetEnteredVision = delegate { };
	//public event Action OnTargetExitedVision = delegate { };

	public float ViewRadius { get { return viewRadius; } }
	public float ViewAngle { get { return viewAngle; } }
	public List<Transform> VisibleTargets { get { return visibleTargets; } }

	[SerializeField] private float viewRadius;
	[SerializeField, Range(0, 360)] private float viewAngle;
    [SerializeField, Range(0, .4f)] private float coneHeight = 0.15f;
	//[SerializeField] private List<LayerMask> targetMasks;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private float meshResolution;
	[SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Transform root;
    [SerializeField] private float lerpFactor = 2f;

	private  Mesh viewMesh;
	private List<Transform> visibleTargets = new List<Transform>();
	private LayerMask targetMask;
	private bool playerInVision = false;
	private NavMeshPath path;

    private float bloomSpeed;
    private Color color;

    private float targetViewRadius;
    private float targetViewAngle;
    private Color targetColor;
    private float targetBloomSpeed;

    private void Awake()
    {
        color = mesh.material.color;
        bloomSpeed = mesh.material.GetFloat("_BloomSpeed");

        targetViewRadius = viewRadius;
        targetViewAngle = viewAngle;
        targetColor = color;
        targetBloomSpeed = bloomSpeed;
    }

    private void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
        
		StartCoroutine(FindTargetsWithDelay(0.05f));
    }

	private void Update()
	{
		//FindVisibleTargets();

		//Debug.Log(visibleTargets.Count);
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

        if (viewRadius != targetViewRadius)
        {
            viewRadius = Mathf.Lerp(viewRadius, targetViewRadius, Time.deltaTime * lerpFactor);
        }
        if (viewAngle != targetViewAngle)
        {
            viewAngle = Mathf.Lerp(viewAngle, targetViewAngle, Time.deltaTime * lerpFactor);
        }
        if (color != targetColor)
        {
            color = Color.Lerp(color, targetColor, Time.deltaTime * lerpFactor);
            mesh.material.color = color;
        }
        if (bloomSpeed != targetBloomSpeed)
        {
            bloomSpeed = Mathf.Lerp(bloomSpeed, targetBloomSpeed, Time.deltaTime * lerpFactor);
            mesh.material.SetFloat("_BloomSpeed", bloomSpeed);
        }

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
		targetColor = c;
	}

    /// <summary>
    /// Changes the "pulse" rate on the vision cone shader. 0.3 = normal, 0.6 = fast, 0.15 = slow
    /// </summary>
    /// <param name="rate"></param>
    public void ChangePulseRate(float rate)
    {
        targetBloomSpeed = rate;
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
	/// Changes the radius of the vision cone.
	/// </summary>
	public void ChangeRadius(float radius)
	{
		targetViewRadius = radius;
	}

	/// <summary>
	/// Changes the view angle of the vision cone.
	/// </summary>
	public void ChangeViewAngle(float angle)
	{
		targetViewAngle = angle;
	}

	public Transform GetClosestTarget()
	{
		if(visibleTargets.Count == 0)
			return null;
			
		float closestDistance = 10000f;
        Transform closestTarget = visibleTargets[0];
		path = new NavMeshPath();

        foreach (Transform t in visibleTargets)
        {
			if(t == null)
				continue; 
				
            GetComponentInParent<NavMeshAgent>().CalculatePath(new Vector3(t.transform.position.x, 0.0f, t.transform.position.z), path); // Calculate the NavMesh path to the object

            if(path.status == NavMeshPathStatus.PathComplete) // Make sure it's a valid path. (So it doesn't target units in unreachable areas.)
            {
                float distance = GetPathDistance(path.corners);

                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = t;
                }
            }
        }

		return closestTarget;
	}

	/// <summary>
    /// Given an array of Vector3's (the corners of the path),
    /// This function will return the total distance of the path.
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    private float GetPathDistance(Vector3[] corners)
    {
        float totalDistance = 0;

        for(int i = 0; i < corners.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(corners[i], corners[i + 1]);
        }

        return totalDistance;
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

			if(target == root) // Don't count itself.
            {
                continue;
            }
				

			Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);	
			Vector3 dirToTarget = (targetPosition - transform.position).normalized;
			
			// Check if the target is within the view angle
			if(Vector3.Angle(dirToTarget, transform.forward) < viewAngle / 2)
			{
				float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

				if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)) // No obstacles are in the way
				{
					visibleTargets.Add(target);
				}
			}
		}

		//Debug.Log(visibleTargets.Count);
	}

	private void DrawVisionCone()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;

		List<Vector3> viewPoints = new List<Vector3>();

        int vertexCount = stepCount * 2 + 3;
        Vector2[] UV = new Vector2[vertexCount];
        UV[0] = new Vector2(0, 0.5f);
        for (int i = 0; i <= stepCount; i++)
		{
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle, coneHeight * Mathf.Sin((i * Mathf.PI)/stepCount));
            UV[(i * 2) + 1] = new Vector2(newViewCast.dst / viewRadius, (float)i / (float)stepCount);
            viewPoints.Add(newViewCast.point);

            newViewCast = ViewCast(angle, -coneHeight * Mathf.Sin((i * Mathf.PI) / stepCount));
            UV[(i * 2) + 2] = new Vector2(newViewCast.dst / viewRadius, (float)i / (float)stepCount);
            viewPoints.Add(newViewCast.point);
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
        }
        //for (int i = stepCount; i > 0; i--)
        //{
        //    float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
        //    ViewCastInfo newViewCast = ViewCast(angle, -coneHeight * Mathf.Sin((i * Mathf.PI) / stepCount));
        //    UV[stepCount + i + 1] = new Vector2(newViewCast.dst / viewRadius, (float)i / (float)stepCount);
        //    viewPoints.Add(newViewCast.point);
        //    //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
        //}


        Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 1) * 3];

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
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i+1;
                triangles[i * 3 + 2] = 1;
            }
		}

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
        viewMesh.uv = UV;
		viewMesh.RecalculateNormals();
	}

	public ViewCastInfo ViewCast(float globalAngle, float ymod)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
        dir.y += ymod;
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
