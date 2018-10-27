using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShowPathing : MonoBehaviour
{
    private NavMeshAgent agent;
    private LineRenderer lr;
    
    void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        lr = gameObject.AddComponent<LineRenderer>();
	}
	
	void Update ()
    {
        DrawPath(GetComponent<Unit>());
	}

    /// <summary>
    /// Draws the path of the navmesh agent.
    /// </summary>
    private void DrawPath(Unit u)
    {
        NavMeshPath path = agent.path;

        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.startColor = Color.green;
        lr.endColor = Color.red;
        lr.material = new Material(Shader.Find("Particles/Additive"));

        lr.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            lr.SetPosition(i, path.corners[i]);
        }
    }
}
