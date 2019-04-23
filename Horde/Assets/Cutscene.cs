using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{

    public GameObject FinalScene;
    GameObject Player;
    GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider playa)
    {
        Player.SetActive(false);
        Camera.SetActive(false);
        FinalScene.SetActive(true);
    }
}
