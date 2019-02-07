using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MaterialSwapTrigger : MonoBehaviour {

    /// <summary>
    /// set this to be the parent object of all the meshes you want to have the materials changed on
    /// </summary>
    public GameObject MeshGroupParent;
    /// <summary>
    /// Set this to the materials you want the meshes to use after the swap
    /// Some meshes use more than one material which is why this is an array
    /// </summary>
    public Material[] Mats;

    private Material[] originalMats;

    private void Start()
    {
        originalMats = MeshGroupParent.GetComponentInChildren<MeshRenderer>().materials;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MeshRenderer[] meshRenderers = MeshGroupParent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.materials = Mats;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            MeshRenderer[] meshRenderers = MeshGroupParent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.materials = originalMats;
            }
        }
    }
}
