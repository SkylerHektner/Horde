using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClipList
{
    public string Name;
    public List<AudioClip> Clips;
}

[System.Serializable]
public class AdvancedSoundEffect : MonoBehaviour
{
    public List<ClipList> Clips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayEventSound(string name, int clipnumber)
    {
        int index = GetNameIndex(name);
        ClipList cliplist = Clips[index];
        AudioClip clip;
        if(clipnumber < 0)
        {
            clip = GetRandomClip(index);
        }
        else
        {
            clip = cliplist.Clips[clipnumber];
        }

        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(int index)
    {
        return Clips[index].Clips[Random.Range(0, Clips[index].Clips.Count)];
    }

    private AudioClip GetRandomClip()
    {
        int index = Random.Range(0, Clips.Count);
        return Clips[index].Clips[Random.Range(0, Clips[index].Clips.Count)];
    }

    private int GetNameIndex(string name)
    {
        for (int i = 0; i < Clips.Count; i++)
        {
            if(Clips[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }
}
