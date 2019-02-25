using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> GetPatrolPoints()
    {
        List<Transform> patrolPoints = new List<Transform>();

        for(int i = 0; i < transform.childCount; i++)
        {
            patrolPoints.Add(transform.GetChild(i));
        }

        return patrolPoints;
    }
}
