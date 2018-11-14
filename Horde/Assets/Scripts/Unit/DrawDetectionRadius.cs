using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDetectionRadius : MonoBehaviour 
{
	private Unit u;
    private int segments = 50;
	private float radius;

    private LineRenderer lr;

    void Start ()
    {
		u = GetComponent<Unit>();

        radius = u.DetectionRange / 2;

        Debug.Log(radius);
		
        lr = gameObject.AddComponent<LineRenderer>();

        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;

		lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.material = new Material(Shader.Find("Particles/Additive"));

        CreatePoints ();
    }

    void CreatePoints ()
    {
        float x;
        //float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

            lr.SetPosition (i,new Vector3(x,0,z) );

            angle += (360f / segments);
        }
    }
}
