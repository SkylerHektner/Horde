using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public int NumberOfFlashes;
    public AnimationCurve FadeIn;
    public AnimationCurve FadeOut;
    public float FlashSpeed;

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
        if(FlashSpeed < 1)
        {
            FlashSpeed = 1;
        }

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
        Color text_color_inv = text.color;
        Color text_color = text_color_inv;
        text_color.a = 1;

        Color img_color_inv = imgage.color;
        Color img_color = img_color_inv;
        img_color.a = 1;

        Color bg_color_inv = bg.color;
        Color bg_color = bg_color_inv;
        bg_color.a = 1;

        while(currentFlash < NumberOfFlashes)
        {
            while(text.color != text_color_inv && imgage.color != img_color_inv && bg.color != bg_color_inv)
            {
                text.color = Color.Lerp(text.color, text_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
                imgage.color = Color.Lerp(imgage.color, img_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
                bg.color = Color.Lerp(bg.color, bg_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
                yield return null;
            }
            text.color = text_color_inv;
            imgage.color = img_color_inv;
            bg.color = bg_color_inv;
            while(text.color != text_color && imgage.color != img_color && bg.color != bg_color)
            {
                text.color = Color.Lerp(text.color, text_color, FadeOut.Evaluate(Time.deltaTime * FlashSpeed));
                imgage.color = Color.Lerp(imgage.color, img_color, FadeOut.Evaluate(Time.deltaTime * FlashSpeed));
                bg.color = Color.Lerp(bg.color, bg_color, FadeOut.Evaluate(Time.deltaTime * FlashSpeed));
                yield return null;
            }
            text.color = text_color;
            imgage.color = img_color;
            bg.color = bg_color;
            currentFlash++;
        }

        while (text.color != text_color_inv && imgage.color != img_color_inv && bg.color != bg_color_inv)
        {
            text.color = Color.Lerp(text.color, text_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
            imgage.color = Color.Lerp(imgage.color, img_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
            bg.color = Color.Lerp(bg.color, bg_color_inv, FadeIn.Evaluate(Time.deltaTime * FlashSpeed));
            yield return null;
        }
        text.color = text_color_inv;
        imgage.color = img_color_inv;
        bg.color = bg_color_inv;
        Debug.Log("DONE WITH FLASH");
    }
}
