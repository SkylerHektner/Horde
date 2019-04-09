using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;

    private int numAnger;
    private int numFear;
    private int numSorrow;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //AIState.OnEmotionStarted += EmotionStart;
        //AIState.OnEmotionEnded += EmotionEnd;
        numAnger = 0;
        numFear = 0;
        numSorrow = 0;
    }

    private void OnEnable()
    {
        AIState.OnEmotionStarted += EmotionStart;
        AIState.OnEmotionEnded += EmotionEnd;
    }

    public void EmotionEnd(string emotion)
    {

    }

    private void EmotionStart(string s)
    {
        //Debug.Log(s);
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
