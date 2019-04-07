using System.Collections;
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
                GameObject go = Instantiate(brokenVersion, transform.position, transform.rotation);
                foreach(Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
                {
                    rb.GetComponent<Rigidbody>().AddExplosionForce(1.0f, transform.position, 5.0f, 3.0f, ForceMode.Impulse);
                }
            }

            if(drop != null)
			    DropItem();
        }

        if(particleEffect != null)
        {
            GameObject effectGO = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Object.Destroy(effectGO, 3.0f);
        }

		Destroy(gameObject);
    }

    private void DropItem()
    {
        Instantiate(drop, transform.position, Quaternion.identity);
    }
}
