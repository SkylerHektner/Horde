using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PatrolPath))]
public class PatrolPathEditor : Editor
{
    PatrolPath patrolPath;

    void OnSceneGUI()
    {
        patrolPath = (PatrolPath)target;

        Handles.color = Color.red;

        for(int i = 0; i < patrolPath.transform.childCount - 1; i++)
        {
            Transform firstChild = patrolPath.transform.GetChild(i);
            Transform secondChild = patrolPath.transform.GetChild(i + 1);

            Handles.DrawDottedLine(firstChild.position, secondChild.position, 5.0f);
        }

        for(int i = 0; i < patrolPath.transform.childCount; i++)
        {
            Transform child = patrolPath.transform.GetChild(i);
            child.position = Handles.FreeMoveHandle(child.position, child.rotation, 1.0f, Vector3.one * 0.5f, Handles.RectangleHandleCap);
            child.position = new Vector3(child.position.x, 0, child.position.z);
        }
    }
}
