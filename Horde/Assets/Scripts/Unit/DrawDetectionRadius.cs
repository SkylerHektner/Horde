using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDetectionRadius : MonoBehaviour 
{
	private Unit u;
    private int segments = 50;
	private float radius;

    //[SerializeField, Tooltip("The default color of the detection range.")]
    //private Color defaultColor;

    //[SerializeField, Tooltip("The color when the player is in detection range.")]
    //private Color detectionColor;

    private LineRenderer lr;

    private void Start ()
    {
		u = GetComponent<Unit>();

        radius = u.DetectionRange / 2;
		
        lr = gameObject.AddComponent<LineRenderer>();

        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;

		lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;
        lr.material = new Material(Shader.Find("Particles/Additive"));

        CreatePoints ();
    }

    private void CreatePoints ()
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

    public void SetToDefaultColor()
    {
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;

        CreatePoints();
    }

    public void SetToDetectioncolor()
    {
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        CreatePoints();
    }
}
