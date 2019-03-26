using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject brokenVersion;

    [Tooltip("The item that you want to drop when this object is broken. Leave it null if you want nothing to drop.")]
    [SerializeField] Transform drop;

    public void Break()
    {
        GameObject go = Instantiate(brokenVersion, transform.position, transform.rotation);
		foreach(Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
		{
			rb.GetComponent<Rigidbody>().AddExplosionForce(1.0f, transform.position, 5.0f, 3.0f, ForceMode.Impulse);
		}

		if(drop != null)
			DropItem();
		
		Destroy(gameObject);
    }

    private void DropItem()
    {
        Instantiate(drop, transform.position, Quaternion.identity);
    }
}
