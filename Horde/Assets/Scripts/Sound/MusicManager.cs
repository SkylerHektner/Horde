using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;

    private int numAnger;
    private int numFear;
    private int numSorrow;
    private int numAlert;
    private int numTotal;

    /*
    private FMOD.Studio.System fmodSystem = FMODUnity.RuntimeManager.StudioSystem;
    private FMOD.Studio.EventInstance bgMusic;
    private FMOD.Studio.ParameterInstance intensity;
    private FMOD.Studio.ParameterInstance anger;
    private FMOD.Studio.ParameterInstance fear;
    private FMOD.Studio.ParameterInstance sorrow;
    */

    private FMODUnity.StudioEventEmitter emitter;

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
        numAnger = 0;
        numFear = 0;
        numSorrow = 0;
        numAlert = 0;
        numTotal = 0;

        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        
        /*bgMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Level 1");
        bgMusic.getParameter("Intensity", out intensity);
        bgMusic.getParameter("Anger", out anger);
        bgMusic.getParameter("Sadness", out sorrow);
        bgMusic.getParameter("Fear", out fear);
        bgMusic.start();*/
        
    }

    private void OnEnable()
    {
        AIState.OnEmotionStarted += EmotionStart;
        AIState.OnEmotionEnded += EmotionEnd;
    }

    public void EmotionEnd(string emotion)
    {
        if(emotion == "Idle")
        {
            return;
        }
        Debug.Log("EMOTION END: " + emotion);
        numTotal--;
        Debug.Log("numTotal: " + numTotal.ToString());
        if (numTotal < 0)
        {
            numTotal = 0;
        }
            switch (emotion)
        {
            case "Anger":
                numAnger--;
                Debug.Log(numAnger);
                if (numAnger <= 0)
                {
                   // anger.setValue(0f);
                    emitter.SetParameter("Anger", 0f);
                }
                break;
            case "Fear":
                numFear--;
                Debug.Log(numFear);
                if(numFear <= 0)
                {
                    //fear.setValue(0f);
                    emitter.SetParameter("Fear", 0f);
                }
                break;
            case "Sadness":
                numSorrow--;
                Debug.Log(numSorrow);
                if(numSorrow <= 0)
                {
                   // sorrow.setValue(0f);
                    emitter.SetParameter("Sadness", 0f);
                }
                break;
            case "Alert":
                numAlert--;
                Debug.Log(numAlert);
                break;
        }
        if(numTotal <= 0)
        {
            //intensity.setValue(0f);
            emitter.SetParameter("Intensity", 0f);
        }
    }

    private void EmotionStart(string emotion)
    {
        //Debug.Log(s);
        Debug.Log("EMOTION START: " + emotion);
        Debug.Log("NUM TOTAL: " + numTotal.ToString());
        numTotal++;
        if(numTotal == 1)
        {
            // intensity.setValue(100f);
            emitter.SetParameter("Intensity", 100f);
        }

        switch(emotion)
        {
            case "Anger":
                numAnger++;
                Debug.Log(numAnger);
                if(numAnger == 1)
                {
                    //  anger.setValue(100f);
                    emitter.SetParameter("Anger", 100f);
                }
                break;

            case "Fear":
                numFear++;
                Debug.Log(numFear);
                if(numFear == 1)
                {
                    //fear.setValue(100f);
                    emitter.SetParameter("Fear", 100f);
                }
                break;

            case "Sadness":
                numSorrow++;
                Debug.Log(numSorrow);
                if(numSorrow == 1)
                {
                    //sorrow.setValue(100f);
                    emitter.SetParameter("Sadness", 100f);
                }
                break;

            case "Alert":
                numAlert++;
                Debug.Log(numAlert);
                break;
        }
    }
}
