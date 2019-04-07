using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private float lowPitchRange = 0.95f;
    [SerializeField] private float highPitchRange = 1.05f;

    private void Awake()
    {
        // Make sure only one instance of this class exists. (Singleton)
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}
