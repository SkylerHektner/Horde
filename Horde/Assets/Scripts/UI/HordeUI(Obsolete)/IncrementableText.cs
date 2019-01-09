using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class IncrementableText : MonoBehaviour {

    private Text t;

    [SerializeField]
    int value;
    [SerializeField]
    int maxValue;

    public int Value { get { return value; } }
    
    private void Start()
    {
        t = GetComponent<Text>();
        t.text = value.ToString();
    }

    public void Increment()
    {
        value++;
        if (value > maxValue)
        {
            value = maxValue;
        }
        t.text = value.ToString();
    }

    public void Decrement()
    {
        value--;
        if (value < 0)
        {
            value = 0;
        }
        t.text = value.ToString();
    }
}
