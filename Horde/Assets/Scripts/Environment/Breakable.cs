﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Tooltip("The broken version that spawns after being hit. Leave null if object doesn't have a broken version (ie. Drums).")]
    [SerializeField] GameObject brokenVersion;

    [Tooltip("The item that you want to drop when this object is broken. Leave it null if you want nothing to drop.")]
    [SerializeField] Transform drop;

    [Tooltip("The particle effect that you want to spawn when the object gets smashed.")]
    [SerializeField] GameObject particleEffect;

    [Tooltip("The sound effect that you want to play when the object gets smashed.")]
    [SerializeField] AudioClip soundEffect;

    public void Break()
    {
        // Removing the collider first so particles don't collide with it.
        Collider c = GetComponent<Collider>();
        if(c)
            c.enabled = false;

        Drum d = GetComponent<Drum>();
        if(d != null) // This object is a Drum
        {
            d.Explode();
        }
        else // Object is not a drum.
        {
            if(brokenVersion != null)
            {
                GameObject brokenVersionGO = Instantiate(brokenVersion, transform.position, transform.rotation);
                Transform[] brokenPieces = brokenVersionGO.GetComponentsInChildren<Transform>();
                foreach(Transform t in brokenPieces)
                {
                    Object.Destroy(t.gameObject, Random.Range(4.0f, 7.0f));
                }
            }

            if(drop != null)
			    DropItem();
        }

        if(soundEffect != null)
        {
            AudioManager.instance.PlaySoundEffectRandomPitch(soundEffect);
        }

        if(particleEffect != null)
        {
            GameObject effectGO = Instantiate(particleEffect, transform.position, Quaternion.Euler(-90, 0, 0));
            Object.Destroy(effectGO, 3.0f);
        }

		Destroy(gameObject);
    }

    private void DropItem()
    {
        Instantiate(drop, transform.position, Quaternion.identity);
    }
}
