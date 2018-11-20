using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBeats : MonoBehaviour {

    public AudioClip MusicClip1;
    public AudioClip MusicClip2;
    public AudioClip MusicClip3;


    public AudioSource MusicSource;


    // Use this for initialization
    void Start()
    {
        MusicSource.clip = MusicClip1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MusicSource.clip = MusicClip1;
            MusicSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MusicSource.clip = MusicClip2;
            MusicSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MusicSource.clip = MusicClip3;
            MusicSource.Play();
        }
    }
}
