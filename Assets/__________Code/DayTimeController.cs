using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeController : MonoBehaviour
{
    public Light dirLight;
    [Space]
    [Range(0, 1)] public float dayTime = 1;
    [Space]
    public Pipe_SoundsPlay Pipe_SoundsPlay;
    public ClipsCollection bell;

    public Vector2 sunRotationRange = new Vector2(-30, 90);
    public Vector2 envLightRange = new Vector2(0.4f, 1.8f);
    public Vector2 fogColorRange = new Vector2(91, 178);

    float currentDayTime;

    private void Start()
    {
        SetDayTime(dayTime);
    }

    public void SetDayTime(float dayTime, int steps, float deltaTime)
    {
        StartCoroutine(SetDayTime_Enum(dayTime, steps, deltaTime));
        Pipe_SoundsPlay.AddClip(new PlayClipData(bell, Camera.main.transform.position, Camera.main.transform));
    }

    IEnumerator SetDayTime_Enum(float newDayTime, int steps, float deltaTime)
    {
        var diff = newDayTime - currentDayTime;
        var deltaDiff = diff / steps;
        for (int i = 1; i <= steps; i++)
        {
            SetDayTime(currentDayTime + deltaDiff);
            yield return new WaitForSeconds(deltaTime);
        }
    }

    public void SetDayTime(float newDayTime)
    {
        dayTime = newDayTime;
        currentDayTime = newDayTime;

        var sunRotation = GetRangeValue(currentDayTime, sunRotationRange);
        var envLight = GetRangeValue(currentDayTime, envLightRange);
        var fogColor = GetRangeValue(currentDayTime, fogColorRange);
        fogColor = fogColor / 255f;

        dirLight.transform.rotation = Quaternion.Euler(
            sunRotation,
            dirLight.transform.rotation.eulerAngles.y,
            dirLight.transform.rotation.eulerAngles.z);
        RenderSettings.ambientIntensity = envLight;
        RenderSettings.fogColor = new Color(fogColor, fogColor, fogColor);
    }

    private void Update()
    {
        if (currentDayTime != dayTime)
        {
            SetDayTime(dayTime);
        }
    }

    float GetRangeValue(float percent, Vector2 range)
    {
        var fullRange = range.y - range.x;
        var percentRange = fullRange * percent;
        var percentValue = range.x + percentRange;
        return percentValue;
    }
}