using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpellMovement : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform target;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
    public void setTarget(Transform t)
    {
        target = t;
    }

	// Update is called once per frame
	void Update () {
        agent.SetDestination(target.position);
        if((gameObject.transform.position - target.transform.position).sqrMagnitude < 3.0f) ///*agent.stoppingDistance*/ && !agent.pathPending)
        {
            target.gameObject.GetComponent<Unit>().OverrideHeuristics();
            Destroy(gameObject);
        }
	}
}
