using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementShake : MonoBehaviour
{
    public Number N_Velocity;
    float prevVelocity = -12903;
    [Space]
    public float maxVelocityChange = 10f;
    public float speedSensitivity = .5f;
    [Space]
    [Range(0, 1)] public float speedChange;
    [Range(0, 1)] public float speedChangeFreshhold = .5f;
    [Space]
    public Pipe_CamShakes Pipe_CamShakes;
    public float shakeAmplitude = .1f;
    public float shakeDuration = .2f;
    public float shakePerSecond = 2f;

    private void Start()
    {
        prevVelocity = N_Velocity.Value;
    }

    void FixedUpdate()
    {
        var frameVelChange = Mathf.Abs(N_Velocity.Value - prevVelocity);
        var velChangePercentage = frameVelChange / maxVelocityChange;
        if (velChangePercentage > 1) velChangePercentage = 1;

        speedChange = Mathf.Lerp(speedChange, velChangePercentage, speedSensitivity);

        if (speedChange > speedChangeFreshhold)
        {
            var amp = speedChange * shakeAmplitude;
            Pipe_CamShakes.AddCamShake(new ShakeAtributes(amp, shakeDuration, shakePerSecond));
        }

        prevVelocity = N_Velocity.Value;
    }
}
