﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour 
{

    public static ResourceManager Instance;

    public delegate void resourceEmptyListener(ResourceType t);
    public event resourceEmptyListener ResourceEmptyEvent;

    public int Rage { get; set; }
    public int Joy { get; set; }
    public int Sadness { get; set; }
    public int Fear { get; set; }

    [SerializeField] private Material greyscalePostMat;
    [SerializeField] private Material resourceBarRageMat;
    [SerializeField] private Material resourceBarFearMat;
    [SerializeField] private Material resourceBarJoyMat;
    [SerializeField] private Material resourceBarDevotionMat;

    private int maxRage;
    private int maxJoy;
    private int maxSadness;
    private int maxFear;

    private void Awake ()
    {
        Instance = this;

        Rage = PlayerPrefs.GetInt("Anger", 0);
        Fear = PlayerPrefs.GetInt("Fear", 0);
        Sadness = PlayerPrefs.GetInt("Sadness", 0);
        Joy = PlayerPrefs.GetInt("Joy", 0);
    }

    private void Start()
    {
        maxRage = GameManager.Instance.Player.PlayerSettings.MaxJuice;
        maxJoy = GameManager.Instance.Player.PlayerSettings.MaxJuice;
        maxSadness = GameManager.Instance.Player.PlayerSettings.MaxJuice;
        maxFear = GameManager.Instance.Player.PlayerSettings.MaxJuice;

        updateResourceBars();
        updateGreyscaleEffect();
    }

    public void updateResourceBars()
    {
        resourceBarRageMat.SetFloat("_Range", (float)Rage / maxRage);
        resourceBarFearMat.SetFloat("_Range", (float)Fear / maxFear);
        resourceBarJoyMat.SetFloat("_Range", (float)Joy / maxJoy);
        resourceBarDevotionMat.SetFloat("_Range", (float)Sadness / maxSadness);
    }

    public void updateGreyscaleEffect()
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
                Rage -= GameManager.Instance.Player.PlayerSettings.CostPerShot;
                break;
            case ResourceType.Fear:
                if (Fear == 0)
                    return false;
                Fear -= GameManager.Instance.Player.PlayerSettings.CostPerShot;
                break;
            case ResourceType.Sadness:
                if (Sadness == 0)
                    return false;
                Sadness -= GameManager.Instance.Player.PlayerSettings.CostPerShot;
                break;
            case ResourceType.Joy:
                if (Joy == 0)
                    return false;
                Joy -= GameManager.Instance.Player.PlayerSettings.CostPerShot;
                break;
        }

        updateGreyscaleEffect();
        updateResourceBars();

        return true;
    }

    /// <summary>
    /// Checks whether there is any of a given emotion left
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CanSpendEmotion(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Rage:
                if (Rage == 0)
                    return false;
                break;
            case ResourceType.Fear:
                if (Fear == 0)
                    return false;
                break;
            case ResourceType.Sadness:
                if (Sadness == 0)
                    return false;
                break;
            case ResourceType.Joy:
                if (Joy == 0)
                    return false;
                break;
        }

        return true;
    }

    /// <summary>
    /// Adds a given amount to a given emotion.
    /// If that amount is more than the max allowed for that
    /// emotion, adds up the max
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    public void AddEmotion(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Rage:
                Rage += amount;
                break;
            case ResourceType.Fear:
                Fear += amount;
                break;
            case ResourceType.Sadness:
                Sadness += amount;
                break;
            case ResourceType.Joy:
                Joy += amount;
                break;
        }

        if (Rage > maxRage)
            Rage = maxRage;
        if (Fear > maxFear)
            Fear = maxFear;
        if (Sadness > maxSadness)
            Sadness = maxSadness;
        if (Joy > maxJoy)
            Joy = maxJoy;

        updateResourceBars();
        updateGreyscaleEffect();
    }
}

public enum ResourceType
{
    Rage,
    Joy,
    Fear,
    Sadness
}
