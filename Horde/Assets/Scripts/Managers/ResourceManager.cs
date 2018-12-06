using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance;

    public delegate void resourceEmptyListener(ResourceType t);
    public event resourceEmptyListener ResourceEmptyEvent;


    public float baseSpellCost = 20f;

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

    /// <summary>
    /// Spends the emotions of a given list of heuristics
    /// </summary>
    /// <param name="b"></param>
    public void SpendEmotion(List<HInterface.HType> b)
    {
        float tempRage = Rage;
        float tempJoy = Joy;
        float tempDevotion = Devotion;
        float tempFear = Fear;
        subtractEmotions(b, ref tempRage, ref tempDevotion, ref tempJoy, ref tempFear);

        Rage = tempRage;
        if (Rage < 0)
        {
            ResourceEmptyEvent(ResourceType.Rage);
        }
        Joy = tempJoy;
        if (Joy < 0)
        {
            ResourceEmptyEvent(ResourceType.Joy);
        }
        Devotion = tempDevotion;
        if (Devotion < 0)
        {
            ResourceEmptyEvent(ResourceType.Devotion);
        }
        Fear = tempFear;
        if (Fear < 0)
        {
            ResourceEmptyEvent(ResourceType.Fear);
        }

        updateGreyscaleEffect();
        updateResourceBars();
    }

    /// <summary>
    /// subtracts the emotions of a given list of heuristics from a set of references floats
    /// </summary>
    /// <param name="b"></param>
    /// <param name="rage"></param>
    /// <param name="devotion"></param>
    /// <param name="joy"></param>
    /// <param name="fear"></param>
    private void subtractEmotions(List<HInterface.HType> b, ref float rage, ref float devotion, ref float joy, ref float fear)
    {
        float rageCost = 0;
        float devotionCost = 0;
        float fearCost = 0;
        float joyCost = 0;
        foreach (HInterface.HType h in b)
        {
            switch (heuristicCosts.GetEmotion(h))
            {
                case ResourceType.Devotion:
                    devotionCost += 1;
                    break;
                case ResourceType.Joy:
                    joyCost += 1;
                    break;
                case ResourceType.Rage:
                    rageCost += 1;
                    break;
                case ResourceType.Fear:
                    fearCost += 1;
                    break;
            }
        }
        rage -= (rageCost / b.Count) * baseSpellCost;
        devotion -= (devotionCost / b.Count) * baseSpellCost;
        joy -= (joyCost / b.Count) * baseSpellCost;
        fear -= (fearCost / b.Count) * baseSpellCost;
    }

    /// <summary>
    /// Tests whether or not you can cast the given list of heuristics. Returns true if you can
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool HasEmotion(List<HInterface.HType> list)
    {
        float tempRage = Rage;
        float tempJoy = Joy;
        float tempDevotion = Devotion;
        float tempFear = Fear;
        subtractEmotions(list, ref tempRage, ref tempDevotion, ref tempJoy, ref tempFear);
        if (tempRage < 0 || tempDevotion < 0 || tempJoy < 0 || tempFear < 0)
        {
            return false;
        }
        return true;
    }
}
