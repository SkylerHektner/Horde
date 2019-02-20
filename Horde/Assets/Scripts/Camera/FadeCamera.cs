using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCamera : MonoBehaviour
{
    public bool isBlack = false;
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 1.0f), new Keyframe(0.5f, 1.0f),new Keyframe(0.75f, 0.95f), new Keyframe(1, 0));
    public AnimationCurve introFadeCurve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(0.5f, 0.5f), new Keyframe(1, 0));
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;


    public void Reset()
    {
        _done = false;
        _alpha = 0;
        _time = 0;
        introFadeCurve = FadeCurve;
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade()
    {
        _done = false;
        _alpha = 0;
        _time = 0;

    }

    public void OnGUI()
    {
        if (_done) return;
        if (_texture == null)
        {
            _texture = new Texture2D(1, 1);
        }

        _texture.SetPixel(0, 0, new Color(0, 0, 0, _alpha));
        _texture.Apply();

        _time += Time.deltaTime/6;
        _alpha = introFadeCurve.Evaluate(_time);
        if(_alpha == 1)
        {
            isBlack = true;
        }
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        if (_alpha <= 0)
        {
            _done = true;
            isBlack = false;
        }
    }
}