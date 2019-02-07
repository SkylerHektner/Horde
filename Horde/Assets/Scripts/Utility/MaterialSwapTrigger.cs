using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MaterialSwapTrigger : MonoBehaviour {

    /// <summary>
    /// set this to be the parent object of all the meshes you want to have the materials changed on
    /// </summary>
    public GameObject MeshGroupParent;
    private Renderer[] renderers;
    /// <summary>
    /// Set this to the materials you want the meshes to use after the swap
    /// Some meshes use more than one material which is why this is an array
    /// </summary>
    public Shader transparentShader;
    private List<Shader[]> originalShaders;

    public float targetAlpha = 0.2f;
    private float currentAlpha = 1f;
    private float fadeTimer = 0f;

    private void Start()
    {
        renderers = MeshGroupParent.GetComponentsInChildren<Renderer>();
        originalShaders = new List<Shader[]>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Shader[] ar = new Shader[renderers[i].materials.Length];
            Material[] mats = renderers[i].materials;
            for (int x = 0; x < mats.Length; x++)
            {
                ar[x] = mats[x].shader;
            }
            originalShaders.Add(ar);
        }
    }

    private void Update()
    {
        if (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] mats = renderers[i].materials;
                for (int x = 0; x < mats.Length; x++)
                {
                    currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime);
                    mats[x].SetFloat("_Alpha", currentAlpha);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] mats = renderers[i].materials;
                for (int x = 0; x < mats.Length; x++)
                {
                    mats[x].shader = transparentShader;
                    mats[x].SetFloat("_Alpha", 1f);
                }
            }
            currentAlpha = 1f;
            fadeTimer = 3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] mats = renderers[i].materials;
                for (int x = 0; x < mats.Length; x++)
                {
                    mats[x].shader = originalShaders[i][x];
                }
            }
            fadeTimer = 0f;
        }
    }
}
