using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -- Heuristic: Explode --
/// 
/// Dashes to the target and explodes.
/// Deals damage in a radius, including damage to allies.
/// 
/// Destroys the unit.
/// 
/// </summary>
public class H_Explode : Heuristic
{
    [SerializeField, Tooltip("How large the explosion is.")]
    private float explosionSize = 6;

    [SerializeField, Tooltip("How much damage the explosion deals.")]
    private int explosionDamage = 3;

    private float chargeSpeed = 25;

    public GameObject explosionPrefab;

    public override void Init()
    {
        base.Init();
        explosionPrefab = Resources.Load<GameObject>("ExplosionParticleSystem");
    }

    public override void Execute()
    {
        if (unit.CurrentTarget == null)
            Resolve();

        // Charge towards the current target 
        transform.position = Vector3.MoveTowards(transform.position, unit.CurrentTarget.transform.position, chargeSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, unit.CurrentTarget.transform.position) < 1)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionSize);

            // Get the Unit components from the object in range and subtract their health
            foreach (Collider c in hitColliders)
            {
                if(c.gameObject.tag == "TeamOneUnit" || c.gameObject.tag == "TeamTwoUnit")
                    c.gameObject.GetComponent<Unit>().TakeDamage(explosionDamage); // Subtract their health
            }
            Instantiate(explosionPrefab).transform.position = transform.position;
            Destroy(gameObject); // The unit sploded.
            Resolve();
        }
    }

    public override void Resolve()
    {
        base.Resolve();
    }
}
