using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class TorchFlicker : MonoBehaviour {

    private float startingBrightness;
    private float targetBrightness;
    private Light torchLight;

    [SerializeField]
    private float deviationMagnitude = 0.2f;
    [SerializeField]
    private float flickerSpeed = 5f;

	private void Start ()
    {
        torchLight = GetComponent<Light>();
        startingBrightness = torchLight.intensity;
        targetBrightness = startingBrightness;
    }
	
	private void Update ()
    {
		if (Mathf.Abs(targetBrightness - torchLight.intensity) > 0.1f)
        {
            torchLight.intensity = Mathf.Lerp(torchLight.intensity, targetBrightness, Time.deltaTime * flickerSpeed);
        }
        else
        {
            targetBrightness = startingBrightness + startingBrightness * (Random.value - 0.5f) * deviationMagnitude;
        }
	}
}
