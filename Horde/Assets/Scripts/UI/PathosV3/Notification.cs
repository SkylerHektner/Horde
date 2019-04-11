using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public int NumberOfFlashes;
    public float FlashesPerSecond;
    public float Smothness;

    private Text text;
    private Image imgage;
    private Image bg;
    private float timePerFlash;
    private float waitTime;
    private float increment;

    // Start is called before the first frame update
    void Start()
    {
        if(NumberOfFlashes < 1)
        {
            NumberOfFlashes = 1;
        }
        if(FlashesPerSecond < 1)
        {
            FlashesPerSecond = 1;
        }

        timePerFlash = 1 / FlashesPerSecond;
        waitTime = timePerFlash / Smothness;
        increment = 255 / waitTime;

        text = GetComponentInChildren<Text>();
        imgage = GetComponentInChildren<Image>();
        bg = GetComponent<Image>();

        PathosUI.instance.NotificationEvent.AddListener(FlashNotification);

        SetAlphas(0);
    }

    private void SetAlphas(int alpha)
    {
        Color t_color = text.color;
        t_color.a = alpha;
        text.color = t_color;

        Color i_color = imgage.color;
        i_color.a = alpha;
        imgage.color = i_color;

        Color bg_color = bg.color;
        bg_color.a = alpha;
        bg.color = bg_color;
    }


    public void FlashNotification()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        int currentFlash = 0;
        int currentAlpha = 0;

        while(currentFlash < NumberOfFlashes)
        {
            Debug.Log("FLASHING");
            while (currentAlpha < 255)
            {
                currentAlpha += (int)increment;

                SetAlphas(currentAlpha);

                yield return new WaitForSeconds(waitTime);
            }
            Debug.Log("Show");
            while (currentAlpha > 0)
            {
                currentAlpha -= (int)increment;

                SetAlphas(currentAlpha);

                yield return new WaitForSeconds(waitTime);
            }
            Debug.Log("HIDE");
            currentFlash++;
        }
        Debug.Log("DONE WITH FLASH");
    }
}
