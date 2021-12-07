using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowerEmulator : MonoBehaviour
{
    public ShakeCamera Addon_CamShake;
    [Space]
    public Number N_Velocity;
    float prevVelocity = -12903;
    Vector3 prevVelocityDirection;
    [Space]
    public float positionSpeedSencitivity = .1f;
    public float speedSencitivity = .1f;
    public float maxVelocityOffset = .1f;
    public float maxVelocityChange = .6f;

    Vector3 targetPosition;

    private void Start()
    {
        prevVelocityDirection = PlayerSinglton.PlayerTransform.forward;
        targetPosition = transform.localPosition;
        prevVelocity = N_Velocity.Value;
    }

    void FixedUpdate()
    {
        var velChange = Mathf.Abs(N_Velocity.Value - prevVelocity);
        var velChangePercentage = velChange / maxVelocityChange;
        velChangePercentage = Mathf.Clamp(velChangePercentage, 0, 1);

        var velDirection = PlayerSinglton.Velocity.normalized;
        var velChangeDirection = velDirection - prevVelocityDirection;
        var localVelocity = (Vector3)(transform.worldToLocalMatrix * velChangeDirection.normalized);
        var speedOffset = localVelocity * maxVelocityOffset * velChangePercentage;

        var shakeOffset = Vector3.zero;
        if (Addon_CamShake != null)
        {
            shakeOffset += Addon_CamShake.CurrentDisplacement;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition - speedOffset + shakeOffset, positionSpeedSencitivity);

        prevVelocity = Mathf.Lerp(prevVelocity, N_Velocity.Value, speedSencitivity);
        prevVelocityDirection = velDirection;
    }
}
