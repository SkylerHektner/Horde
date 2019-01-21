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
    /// set this to the material you want the meshes to use after the swap
    /// </summary>
    public Material Mat;

    private Material originalMat;

    private void Start()
    {
        originalMat = MeshGroupParent.GetComponentInChildren<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MeshRenderer[] meshRenderers = MeshGroupParent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.material = Mat;
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
                mr.material = originalMat;
            }
        }
    }
}
