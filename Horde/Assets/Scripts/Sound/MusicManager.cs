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
        GameManager.Instance.ChangeRoomEvent.AddListener(ResetMusic);
        
    }

    private void ResetMusic()
    {
        emitter.SetParameter("Anger", 0f);
        emitter.SetParameter("Fear", 0f);
        emitter.SetParameter("Sadness", 0f);
        emitter.SetParameter("Intensity", 0f);

        numAlert = 0;
        numAnger = 0;
        numFear = 0;
        numSorrow = 0;
        numTotal = 0;
    }

    private void OnEnable()
    {
        AIState.OnEmotionStarted += EmotionStart;
        AIState.OnEmotionEnded += EmotionEnd;
    }

    public void EmotionEnd(string emotion)
    {

        switch (emotion)
        {
            case "Anger":
                numAnger--;
                Debug.Log(numAnger);
                if (numAnger <= 0)
                {
                    emitter.SetParameter("Anger", 0f);
                }
                numTotal--;
                if (numTotal < 0)
                {
                    numTotal = 0;
                }
                break;
            case "Fear":
                numFear--;
                Debug.Log(numFear);
                if(numFear <= 0)
                {
                    emitter.SetParameter("Fear", 0f);
                }
                numTotal--;
                if (numTotal < 0)
                {
                    numTotal = 0;
                }
                break;
            case "Sadness":
                numSorrow--;
                Debug.Log(numSorrow);
                if(numSorrow <= 0)
                {
                    emitter.SetParameter("Sadness", 0f);
                }
                numTotal--;
                if (numTotal < 0)
                {
                    numTotal = 0;
                }
                break;
            case "Alert":
                numAlert--;
                numTotal--;
                if (numTotal < 0)
                {
                    numTotal = 0;
                }
                Debug.Log(numAlert);
                break;
        }
        if(numTotal <= 0)
        {
            emitter.SetParameter("Intensity", 0f);
        }
    }

    private void EmotionStart(string emotion)
    {
        switch(emotion)
        {
            case "Anger":
                numTotal++;
                if (numTotal == 1)
                {
                    emitter.SetParameter("Intensity", 100f);
                }
                numAnger++;
                Debug.Log(numAnger);
                if(numAnger == 1)
                {
                    emitter.SetParameter("Anger", 100f);
                }
                break;

            case "Fear":
                numTotal++;
                if (numTotal == 1)
                {
                    emitter.SetParameter("Intensity", 100f);
                }
                numFear++;
                Debug.Log(numFear);
                if(numFear == 1)
                {
                    emitter.SetParameter("Fear", 100f);
                }
                break;

            case "Sadness":
                numTotal++;
                if (numTotal == 1)
                {
                    emitter.SetParameter("Intensity", 100f);
                }
                numSorrow++;
                Debug.Log(numSorrow);
                if(numSorrow == 1)
                {
                    emitter.SetParameter("Sadness", 100f);
                }
                break;

            case "Alert":
                numAlert++;
                numTotal++;
                if (numTotal == 1)
                {
                    emitter.SetParameter("Intensity", 100f);
                }
                Debug.Log(numAlert);
                break;
        }
    }
}
