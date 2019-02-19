using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour 
{

    public static ResourceManager Instance;

    public delegate void resourceEmptyListener(ResourceType t);
    public event resourceEmptyListener ResourceEmptyEvent;

    [SerializeField] public int maxRage = 10;
    [SerializeField] public int maxJoy = 10;
    [SerializeField] public int maxSadness = 10;
    [SerializeField] public int maxFear = 10;
    public int Rage { get; private set; }
    public int Joy { get; private set; }
    public int Sadness { get; private set; }
    public int Fear { get; private set; }

    [SerializeField]
    private Material greyscalePostMat;

    [SerializeField]
    private Material resourceBarRageMat;
    [SerializeField]
    private Material resourceBarFearMat;
    [SerializeField]
    private Material resourceBarJoyMat;
    [SerializeField]
    private Material resourceBarDevotionMat;

    public enum ResourceType
    {
        Rage,
        Joy,
        Fear,
        Sadness
    }

    private void Awake ()
    {
        Instance = this;
        Rage = maxRage;
        Sadness = maxSadness;
        Joy = maxJoy;
        Fear = maxFear;
        updateResourceBars();
        updateGreyscaleEffect();
    }

    private void updateResourceBars()
    {
        resourceBarRageMat.SetFloat("_Range", (float)Rage / maxRage);
        resourceBarFearMat.SetFloat("_Range", (float)Fear / maxFear);
        resourceBarJoyMat.SetFloat("_Range", (float)Joy / maxJoy);
        resourceBarDevotionMat.SetFloat("_Range", (float)Sadness / maxSadness);
    }

    private void updateGreyscaleEffect()
    {
        float sum = 0;
        sum += 1 - (float)Rage / maxRage;
        sum += 1 - (float)Fear / maxFear;
        sum += 1 - (float)Joy / maxJoy;
        sum += 1 - (float)Sadness / maxSadness;
        sum *= 0.25f;
        greyscalePostMat.SetFloat("_GreyAmountRed", sum);
        greyscalePostMat.SetFloat("_GreyAmountGreen", sum);
        greyscalePostMat.SetFloat("_GreyAmountBlue", sum);
    }

    /// <summary>
    /// Spends the emotion of the passed in resource type.
    /// Returns true if succesful, false otherwise
    public bool TrySpendEmotion(ResourceType type)
    {
        switch(type)
        {
            case ResourceType.Rage:
                if (Rage == 0)
                    return false;
                Rage -= 1;
                break;
            case ResourceType.Fear:
                if (Fear == 0)
                    return false;
                Fear -= 1;
                break;
            case ResourceType.Sadness:
                if (Sadness == 0)
                    return false;
                Sadness -= 1;
                break;
            case ResourceType.Joy:
                if (Joy == 0)
                    return false;
                Joy -= 1;
                break;
        }

        updateGreyscaleEffect();
        updateResourceBars();

        return true;
    }
}
