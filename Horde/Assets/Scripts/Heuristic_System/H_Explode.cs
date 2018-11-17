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
    private int explosionDamage = 500;

    public GameObject explosionPrefab;

    public override void Init()
    {
        base.Init();
        explosionPrefab = Resources.Load<GameObject>("ExplosionParticleSystem");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionSize);

        // Get the Unit components from the object in range and subtract their health
        foreach (Collider c in hitColliders)
        {
            Debug.Log(c.tag);
            if (c.gameObject.tag == "TeamTwoUnit" && c.gameObject != gameObject)
            {
                c.gameObject.GetComponent<Unit>().UnitController.TakeDamage(explosionDamage);
            }
        }
        Instantiate(explosionPrefab).transform.position = transform.position;
        unit.UnitController.TakeDamage(explosionDamage); // The unit sploded.
        Resolve();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Resolve()
    {
        base.Resolve();
    }
}
