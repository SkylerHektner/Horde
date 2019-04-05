﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour 
{
	public List<HInterface.HType> Heuristics { get { return heuristics; }  set { heuristics = value; } }

	private ResourceType loadedEmotion;
	private List<HInterface.HType> heuristics;
	private Rigidbody rb;
	private Vector3 oldVel;
	private int bounces = 0;
	private float dartDuration;



    private void Start () 
	{
		rb = GetComponent<Rigidbody>();
		dartDuration = GameManager.Instance.Player.PlayerSettings.DartDuration;
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
            if (enemy.IsDead)
            {
                return;
            }
			//Debug.Log(loadedEmotion);
			switch(loadedEmotion)
			{
				case ResourceType.Rage:
					enemy.ChangeState(new Anger(enemy, dartDuration));
					break;
				case ResourceType.Joy:
					enemy.ChangeState(new Joy(enemy, dartDuration));
					break;
				case ResourceType.Sadness:
					enemy.ChangeState(new Sadness(enemy, dartDuration));
					break;
				case ResourceType.Fear:
					enemy.ChangeState(new Fear(enemy, dartDuration));
					break;
			}
				
			Destroy(gameObject);
		}
		else
		{
			bounces++;
			if(bounces >= 2)
			{
				Destroy(gameObject);
			}

			ContactPoint cp = c.contacts[0];
			rb.velocity = Vector3.Reflect(oldVel, cp.normal).normalized * 125;
		}
	}

	public void LoadEmotion(ResourceType emotion)
	{
		loadedEmotion = emotion;
	}
}
