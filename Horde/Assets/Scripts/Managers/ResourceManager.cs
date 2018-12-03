using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance;

    public delegate void resourceEmptyListener(ResourceType t);
    public event resourceEmptyListener ResourceEmptyEvent;

    public float Rage { get; private set; }
    public float Devotion { get; private set; }
    public float Joy { get; private set; }
    public float Fear { get; private set; }
    [SerializeField]
    private float baseCost = 10;
    [SerializeField]
    private float maxRage = 100;
    [SerializeField]
    private float maxDevotion = 100;
    [SerializeField]
    private float maxJoy = 100;
    [SerializeField]
    private float maxFear = 100;

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

    [SerializeField]
    private HeuristicCosts heuristicCosts;

    public enum ResourceType
    {
        Rage,
        Devotion,
        Joy,
        Fear
    }

    private void Awake ()
    {
        Instance = this;
        Rage = maxRage;
        Devotion = maxDevotion;
        Joy = maxJoy;
        Fear = maxFear;
        updateResourceBars();
        updateGreyscaleEffect();
    }

    private void updateResourceBars()
    {
        resourceBarRageMat.SetFloat("_Range", Rage / maxRage);
        resourceBarFearMat.SetFloat("_Range", Fear / maxFear);
        resourceBarJoyMat.SetFloat("_Range", Joy / maxJoy);
        resourceBarDevotionMat.SetFloat("_Range", Devotion / maxDevotion);
    }
    
    private void updateGreyscaleEffect()
    {
        float sum = 0;
        sum += 1 - Rage / maxRage;
        sum += 1 - Fear / maxFear;
        sum += 1 - Joy / maxJoy;
        sum += 1 - Devotion / maxDevotion;
        sum *= 0.25f;
        greyscalePostMat.SetFloat("_GreyAmountRed", sum);
        greyscalePostMat.SetFloat("_GreyAmountGreen", sum);
        greyscalePostMat.SetFloat("_GreyAmountBlue", sum);
    }

    public void SpendRage(float value)
    {
        Rage -= value;
        updateResourceBars();
        if (Rage <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Rage);
        }
        if(Rage <= 0)
        {
            Rage = 0;
        }
        updateGreyscaleEffect();
    }

    public void SpendDevotion(float value)
    {
        Devotion -= value;
        updateResourceBars();
        if (Devotion <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Devotion);
        }
        if (Devotion <= 0)
        {
            Devotion = 0;
        }
        updateGreyscaleEffect();
    }

    public void SpendJoy(float value)
    {
        Joy -= value;
        updateResourceBars();
        if (Joy <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Joy);
        }
        if (Joy <= 0)
        {
            Joy = 0;
        }
        updateGreyscaleEffect();
    }

    public void SpendFear(float value)
    {
        Fear -= value;
        updateResourceBars();
        if (Fear <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Fear);
        }
        if (Fear <= 0)
        {
            Fear = 0;
        }
        updateGreyscaleEffect();
    }

    public void SpendEmotion(HInterface.HType b)
    {
        heuristicCosts.SpendEmotion(b);
    }

    public bool HasEmotion(List<HInterface.HType> list)
    {
        float tempRage = Rage;
        float tempJoy = Joy;
        float tempDevotion = Devotion;
        float tempFear = Fear;
        foreach (var item in list)
        {
            switch(heuristicCosts.GetEmotion(item))
            {
                case ResourceType.Rage:
                    tempRage -= heuristicCosts.GetCost(item);
                    if (tempRage < 0)
                    {
                        return false;
                    }
                    break;
                case ResourceType.Fear:
                    tempFear -= heuristicCosts.GetCost(item);
                    if (tempFear < 0)
                    {
                        return false;
                    }
                    break;
                case ResourceType.Devotion:
                    tempDevotion -= heuristicCosts.GetCost(item);
                    if(tempDevotion < 0)
                    {
                        return false;
                    }
                    break;
                case ResourceType.Joy:
                    tempJoy -= heuristicCosts.GetCost(item);
                    if (tempJoy < 0)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    [ContextMenu("Test Spending Resources")]
    private void spendResourcesTest()
    {
        SpendRage(25f);
        SpendDevotion(25f);
        SpendJoy(25f);
        SpendFear(25f);
    }


}
