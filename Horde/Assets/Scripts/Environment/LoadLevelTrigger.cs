using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelTrigger : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (nextScene != null && other.tag == "Player")
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
