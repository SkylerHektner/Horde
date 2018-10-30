using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private float targetRimValue = 1.0f;
    private float currentRimValue = 1.0f;
    private float targetLightIntensity = 1.0f;
    private float currentLightIntensity = 1.0f;

    [SerializeField]
    private string missionTitle;
    [SerializeField]
    private string missionDescription;
    [SerializeField]
    private string levelID;

    [SerializeField]
    private Material sphereMat;
    [SerializeField]
    private Light pointLight;
    [SerializeField]
    private float defaultRimIntensity = 1.0f;
    [SerializeField]
    private float hoverRimIntensity = 2.0f;
    [SerializeField]
    private float defaultLightIntensity = 1.0f;
    [SerializeField]
    private float hoverLightIntensity = 2.0f;

    private void Start()
    {
        sphereMat = GetComponent<Renderer>().material;
        currentRimValue = sphereMat.GetFloat("_RimValue");
        currentLightIntensity = pointLight.intensity;
        targetRimValue = defaultRimIntensity;
        targetLightIntensity = defaultLightIntensity;
    }

    private void Update()
    {
        if (currentRimValue != targetRimValue)
        {
            currentRimValue = Mathf.Lerp(currentRimValue, targetRimValue, 5.0f * Time.deltaTime);
            sphereMat.SetFloat("_RimValue", currentRimValue);
        }
        if (currentLightIntensity != targetLightIntensity)
        {
            currentLightIntensity = Mathf.Lerp(currentLightIntensity, targetLightIntensity, 5.0f * Time.deltaTime);
            pointLight.intensity = currentLightIntensity;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CameraController.Instance.SetTargetPos(transform.position.x, transform.position.z);
        CameraController.Instance.setTargetZoom(10);
        CameraController.Instance.lockPanControls = true;
        MissionInfoPanel.Instance.ShowUI(missionTitle, missionDescription, levelID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetRimValue = hoverRimIntensity;
        targetLightIntensity = hoverLightIntensity;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetRimValue = defaultRimIntensity;
        targetLightIntensity = defaultLightIntensity; ;
    }
}
