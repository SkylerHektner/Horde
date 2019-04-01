using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoStreamer : MonoBehaviour {

    private RawImage rawImage;
    private VideoPlayer videoPlayer;

    private bool isPlaying = false;

	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
	}
	
    public void beginPlaying()
    {
        if (!isPlaying)
        {
            Debug.Log("Call Play Part 2");
            StartCoroutine(PlayVideo());
        }
    }

    public void endPlaying()
    {
        if(isPlaying)
        {
            Debug.Log("Stop Video");
            videoPlayer.Stop();
        }
    }

    public void SetVideo(VideoClip texture)
    {
        videoPlayer.clip = texture;
    }

    private IEnumerator PlayVideo()
    {
        Debug.Log("PREPARING VIDEO");
        videoPlayer.Prepare();
        WaitForSeconds waitTime = new WaitForSeconds(1);
        Debug.Log("Loop");
        while(!videoPlayer.isPrepared)
        {
            Debug.Log("SETTING UP");
            yield return waitTime;
            break;
        }
        Debug.Log("SET UP COMPLETE");
        rawImage.texture = videoPlayer.texture;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
        Debug.Log("PLAY WAS CALLED");
    }
}
