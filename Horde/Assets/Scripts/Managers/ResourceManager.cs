﻿using System.Collections;
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

    public enum ResourceType
    {
        Rage,
        Devotion,
        Tranquility
    }

    private void Start ()
    {
        Instance = this;
        Rage = maxRage;
        Devotion = maxDevotion;
        Tranquility = maxTranquility;
        updateSliders();
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
    }

    public void SpendDevotion(float value)
    {
        Devotion -= value;
        updateSliders();
        if (Devotion <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Devotion);
        }
    }

    public void SpendTranquility(float value)
    {
        Tranquility -= value;
        updateSliders();
        if (Tranquility <= 0 && ResourceEmptyEvent != null)
        {
            ResourceEmptyEvent(ResourceType.Tranquility);
        }
    }

    [ContextMenu("Test Spending Resources")]
    private void spendResourcesTest()
    {
        SpendRage(10f);
        SpendDevotion(7f);
        SpendTranquility(5f);
    }
}
