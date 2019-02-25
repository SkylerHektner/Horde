using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount;

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            ResourceManager.Instance.AddEmotion(resourceType, amount);
            Destroy(gameObject);
        }
    }
}
