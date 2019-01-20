using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour 
{
	public List<HInterface.HType> Heuristics { get { return heuristics; }  set { heuristics = value; } }

	private ResourceManager.ResourceType loadedEmotion;
	private List<HInterface.HType> heuristics;
	private Rigidbody rb;
	private Vector3 oldVel;
	private int bounces = 0;
	private float dartDuration = 8.0f;

	private void Start () 
	{
		rb = GetComponent<Rigidbody>();
		//loadedEmotion = ResourceManager.ResourceType.Fear;
	}
	
	private void FixedUpdate () 
	{ 
		// Rotate the projectile along it's projectile path.
		transform.rotation = Quaternion.LookRotation(rb.velocity);
		oldVel = rb.velocity;

		//Debug.Log(loadedEmotion);
	}

	private void OnCollisionEnter(Collision c)
	{
		if(c.gameObject.tag == "Player")
			return;

		if(c.gameObject.tag == "Enemy")
		{
			Enemy enemy = c.gameObject.GetComponent<Enemy>();
			//Debug.Log(loadedEmotion);
			switch(loadedEmotion)
			{
				case ResourceManager.ResourceType.Rage:
					enemy.StartCoroutine(enemy.ChangeStateForDuration(new Anger(enemy), dartDuration));
					break;
				case ResourceManager.ResourceType.Joy:
					enemy.StartCoroutine(enemy.ChangeStateForDuration(new Joy(enemy), dartDuration));
					break;
				case ResourceManager.ResourceType.Sadness:
					enemy.StartCoroutine(enemy.ChangeStateForDuration(new Sadness(enemy), dartDuration));
					break;
				case ResourceManager.ResourceType.Fear:
					enemy.StartCoroutine(enemy.ChangeStateForDuration(new Fear(enemy), dartDuration));
					break;
			}
				
			Destroy(gameObject);
		}
		else
		{
			bounces++;
			if(bounces >= 4)
			{
				Destroy(gameObject);
			}

			ContactPoint cp = c.contacts[0];
			rb.velocity = Vector3.Reflect(oldVel, cp.normal).normalized * 125;
		}
	}

	public void LoadEmotion(ResourceManager.ResourceType emotion)
	{
		loadedEmotion = emotion;
	}
}
