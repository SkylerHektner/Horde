using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Explode : Heuristic
{
    [SerializeField, Tooltip("How large the explosion is.")]
    private float explosionSize = 6;

    [SerializeField, Tooltip("How much damage the explosion deals.")]
    private int explosionDamage = 5;

    public override void Init()
    {
        base.Init();
    }

    public override void Execute()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionSize);

        // Get the Unit components from the object in range and subtract their health
        foreach(Collider c in hitColliders)
        {
            if(c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Unit>().TakeDamage(explosionDamage); // Subtract their health
            }
        }

        Resolve();
    }

    public override void Resolve()
    {
        Destroy(gameObject); // The unit sploded.

        base.Resolve();
    }
}
