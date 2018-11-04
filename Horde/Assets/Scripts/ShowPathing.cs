using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShowPathing : MonoBehaviour
{
    private NavMeshAgent agent;
    private LineRenderer lr;
    private Unit u;
    
    void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        lr = gameObject.AddComponent<LineRenderer>();
        u = GetComponent<Unit>();
	}
	
	void Update ()
    {
        DrawPath(u);
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
            // We need to manually update the last corner 
            // because the path doesn't update while attacking.
            if(i == path.corners.Length - 1) // If it's the last corner
                lr.SetPosition(i, u.CurrentTarget.transform.position);
            else
                lr.SetPosition(i, path.corners[i]);
        }
    }
}
