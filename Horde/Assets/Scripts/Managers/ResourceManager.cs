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
    public float Tranquility { get; private set; }

    [SerializeField]
    private float maxRage = 100;
    [SerializeField]
    private float maxDevotion = 100;
    [SerializeField]
    private float maxTranquility = 100;

    [SerializeField]
    private Slider rageSlider;
    [SerializeField]
    private Slider devotionSlider;
    [SerializeField]
    private Slider tranquilitySlider;

    [SerializeField]
    private Material greyscalePostMat;

    [SerializeField]
    private HeuristicCosts heuristicCosts;

    public enum ResourceType
    {
        Rage,
        Devotion,
        Tranquility
    }

    private void Awake ()
    {
        Instance = this;
        Rage = maxRage;
        Devotion = maxDevotion;
        Tranquility = maxTranquility;
        updateSliders();
        greyscalePostMat.SetFloat("_GreyAmountRed", 1 - Rage / maxRage);
        greyscalePostMat.SetFloat("_GreyAmountGreen", 1 - Devotion / maxDevotion);
        greyscalePostMat.SetFloat("_GreyAmountBlue", 1 - Tranquility / maxTranquility);
    }

    private void updateSliders()
    {
        rageSlider.value = Rage / maxRage;
        devotionSlider.value = Devotion / maxDevotion;
        tranquilitySlider.value = Tranquility / maxTranquility;
    }

    public void SpendRage(float value)
    {
        Rage -= value;
        updateSliders();
        if(Rage <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Rage);
        }
        if(Rage <= 0)
        {
            Rage = 0;
        }
        greyscalePostMat.SetFloat("_GreyAmountRed", 1 - Rage / maxRage);
    }

    public void SpendDevotion(float value)
    {
        Devotion -= value;
        updateSliders();
        if (Devotion <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Devotion);
        }
        if (Devotion <= 0)
        {
            Devotion = 0;
        }
        greyscalePostMat.SetFloat("_GreyAmountGreen", 1 - Devotion / maxDevotion);
    }

    public void SpendTranquility(float value)
    {
        Tranquility -= value;
        updateSliders();
        if (Tranquility <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Tranquility);
        }
        if (Tranquility <= 0)
        {
            Tranquility = 0;
        }
        greyscalePostMat.SetFloat("_GreyAmountBlue", 1 - Tranquility / maxTranquility);
    }

    public void SpendEmotion(HInterface.HType b)
    {
        heuristicCosts.SpendEmotion(b);
    }

    [ContextMenu("Test Spending Resources")]
    private void spendResourcesTest()
    {
        SpendRage(25f);
        SpendDevotion(25f);
        SpendTranquility(25f);
    }
}
