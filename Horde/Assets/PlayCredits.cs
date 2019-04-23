using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCredits : MonoBehaviour
{

    public float DelayTimer = 5f;
    public GameObject Credits;
    Vector3 direction;
    float speed = 10f;
    bool scroolCredits = false;


    // Start is called before the first frame update
    void Awake()
    {
        scroolCredits = true;
        //direction = transform.TransformDirection(new Vector3(0, 1, 0));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (scroolCredits)
        {
            DelayTimer -= Time.smoothDeltaTime; 
        }
        if (DelayTimer <= 0) // when the delay is up, scroll credits
        {
            DelayTimer = 0;
            //Credits.transform.Translate(direction * Time.deltaTime * speed);
        }

    }
}
