using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pipe_CamShakes", menuName = "Pipes/CamShake")]
public class Pipe_CamShakes : ScriptableObject
{
    public List<ShakeAtributes> AwaitingCamShakes { get; } = new List<ShakeAtributes>();

    public void AddCamShake(ShakeAtributes shakeAtributes)
    {
        AwaitingCamShakes.Add(shakeAtributes);
    }
}

[System.Serializable]
public struct ShakeAtributes
{
    public float amplitude;
    public float duration;
    public float shakesPerSecond;

    public ShakeAtributes(float amplitude, float duration, float shakesPerSecond)
    {
        this.amplitude = amplitude;
        this.duration = duration;
        this.shakesPerSecond = shakesPerSecond;
    }
}