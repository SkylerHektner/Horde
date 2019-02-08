﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Drum : MonoBehaviour, IBreakable
{
	[SerializeField] private DrumType drumType;
	[SerializeField] private float explosionRadius;
	[SerializeField] private float effectDuration;

	private MeshRenderer meshRenderer;
	private LayerMask mask;

	void Start () 
	{
		mask = 1 << LayerMask.NameToLayer("Enemy"); // Only affects enemies for now.
	}
	
	void Update () 
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();

		//var tempMaterial = new Material(meshRenderer.sharedMaterial);
		
		switch(drumType)
		{
			case DrumType.Anger:
				meshRenderer.material = Resources.Load<Material>("materials/AngerDrum");
				break;
			case DrumType.Fear:
				meshRenderer.material = Resources.Load<Material>("materials/FearDrum");
				break;
			case DrumType.Sadness:
				meshRenderer.material = Resources.Load<Material>("materials/SadnessDrum");
				break;
			case DrumType.Joy:
				meshRenderer.material = Resources.Load<Material>("materials/JoyDrum");
				break;
			case DrumType.Explosive:
				meshRenderer.material = Resources.Load<Material>("materials/ExplosiveDrum");
				break;
		}
	}

	public void Break()
	{
		Explode();
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	private void Explode()
	{
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius, mask);

		// If the drum type is explosive, just destroy everyone around it.
		if(drumType == DrumType.Explosive)
		{
			foreach(Collider c in objectsInRange)
			{
				Destroy(c.gameObject);
			}
		}
		else
		{
			foreach(Collider c in objectsInRange)
			{
				Enemy enemy = c.GetComponent<Enemy>();

				switch(drumType)
				{
					case DrumType.Anger:
						enemy.ChangeState(new Anger(enemy, effectDuration));
						break;
					case DrumType.Fear:
						enemy.ChangeState(new Fear(enemy, effectDuration));
						break;
					case DrumType.Sadness:
						enemy.ChangeState(new Sadness(enemy, effectDuration));
						break;
					case DrumType.Joy:
						enemy.ChangeState(new Joy(enemy, effectDuration));
						break;
				}
			}
		}

		GameObject bloodExplosion = Instantiate(Resources.Load("ExplosionParticleSystem"), transform.position, Quaternion.identity) as GameObject;

		Destroy(gameObject);
	}

	private enum DrumType { Anger, Sadness, Fear, Joy, Explosive }
}
