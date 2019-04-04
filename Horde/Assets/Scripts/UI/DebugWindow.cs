using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class DebugWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI angerText;
    [SerializeField] TextMeshProUGUI fearText;
    [SerializeField] TextMeshProUGUI sadnessText;

    void Start()
    {
        
    }

    void Update()
    {
        angerText.text =  ResourceManager.Instance.Rage.ToString();
        fearText.text =  ResourceManager.Instance.Fear.ToString();
        sadnessText.text =  ResourceManager.Instance.Sadness.ToString();
    }

    public void IncrementAnger()
    {
        ResourceManager.Instance.Rage ++;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void DecrementAnger()
    {
        ResourceManager.Instance.Rage --;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void IncrementFear()
    {
        ResourceManager.Instance.Fear ++;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void DecrementFear()
    {
        ResourceManager.Instance.Fear --;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void IncrementSadness()
    {
        ResourceManager.Instance.Sadness ++;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void DecrementSadness()
    {
        ResourceManager.Instance.Sadness --;
        ResourceManager.Instance.updateGreyscaleEffect();
        ResourceManager.Instance.updateResourceBars();
    }

    public void TransitionToNextRoom()
    {
        GameManager.Instance.TransitionToNextRoom();
    }

    public void TransitionToPreviousRoom()
    {
        GameManager.Instance.TransitionToPreviousRoom();
    }
}
