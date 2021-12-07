using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementSound : MonoBehaviour
{
    public ClampedValue PM_Forward;
    public ClampedValue PM_Sideways;
    public ClampedValue PM_UP;
    [Space]
    public Number N_Velocity;
    float prevVelocity = -12903;
    public float velocityFreshhold = .5f;
    public float maxVelocity = 160f;
    public float maxVelocityChange = .6f;
    [Space]
    public float engineVolumeSensitivity = .1f;
    public float trackingVolumeSensitivity = .5f;
    public float windVolumeSensitivity = .7f;
    [Space]
    [Range(0, 1)] public float defEngineVolume = .1f;
    [Space]
    [Range(0, 1)] public float engineVolume;
    [Range(0, 1)] public float crackingVolume;
    [Range(0, 1)] public float windVolume;
    [Space]
    [Range(0, 1)] public float maxEngineVolume;
    [Range(0, 1)] public float maxCrackingVolume;
    [Range(0, 1)] public float maxWindVolume;
    [Space]
    public AudioSource engineSound;
    public AudioSource crackingSound;
    public AudioSource windSound;

    private void Start()
    {
        prevVelocity = N_Velocity.Value;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            engineSound.enabled = false;
            windSound.enabled = false;
            crackingSound.enabled= false;
            return;
        }
        else
        {
            engineSound.enabled = true;
            windSound.enabled = true;
            crackingSound.enabled = true;
        }

        var frameVelocity = N_Velocity.Value;
        if (frameVelocity < velocityFreshhold)
            frameVelocity = 0;

        var frameInput = 3 * Mathf.Abs(PM_Forward.Value) + 2 * Mathf.Abs(PM_Sideways.Value) + Mathf.Abs(PM_UP.Value);
        frameInput /= 6;

        var velocityPercentage = frameVelocity / maxVelocity;
        if (velocityPercentage > 1) velocityPercentage = 1;

        var targetEngineVolume = (.1f * frameInput) + (.8f * velocityPercentage) + defEngineVolume;
        engineVolume = Mathf.Lerp(engineVolume, targetEngineVolume, engineVolumeSensitivity);


        var frameVelChange = Mathf.Abs(N_Velocity.Value - prevVelocity);
        var velChangePercentage = frameVelChange / maxVelocityChange;
        if (velChangePercentage > 1) velChangePercentage = 1;

        var targetCrackingVolume = (.98f * velChangePercentage) + (.02f * velocityPercentage);
        crackingVolume = Mathf.Lerp(crackingVolume, targetCrackingVolume, trackingVolumeSensitivity);


        var targetWindVolume = (.25f * velChangePercentage) + (.75f * velocityPercentage);
        windVolume = Mathf.Lerp(windVolume, targetWindVolume, windVolumeSensitivity);

        prevVelocity = N_Velocity.Value;

        engineSound.volume = maxEngineVolume * engineVolume;
        windSound.volume = maxWindVolume * windVolume;
        crackingSound.volume = maxCrackingVolume * crackingVolume;
    }
}
