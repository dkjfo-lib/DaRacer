using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public ShakeCamera Addon_CamShake;
    [Space]
    public float maxDistance = 1;
    public Vector3 offset = Vector2.zero;
    [Space]
    [Range(0f, 1f)] public float stickness = .5f;
    [Range(1f, 2f)] public float screenBorder = 1.2f;
    [Space]
    public bool copyRotation = true;
    [Range(0f, 1f)] public float roationalStickness = .5f;

    Vector3 lastTargetPosition;

    private void Start()
    {
        if (PlayerSinglton.IsGood)
        {
            transform.position = PlayerSinglton.PlayerPosition + offset;
        }
    }
    void Update()
    {
        var targetPosition = GetTargetPosition();
        var diff = transform.position - targetPosition;
        if (diff.sqrMagnitude > maxDistance * maxDistance)
        {
            transform.position = targetPosition + diff.normalized * maxDistance;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, stickness);
        transform.rotation = Quaternion.LookRotation(PlayerSinglton.CamAnchor.forward, PlayerSinglton.CamAnchor.up);
        //if (copyRotation)
        //    transform.rotation = Quaternion.Lerp(transform.rotation, PlayerSinglton.CamRotation, roationalStickness);
    }

    Vector3 GetTargetPosition()
    {
        if (PlayerSinglton.IsGood)
        {
            lastTargetPosition = PlayerSinglton.CamPosition;
        }

        Vector3 targetPosition = lastTargetPosition + offset;

        if (Addon_CamShake != null)
        {
            targetPosition += Addon_CamShake.CurrentDisplacement;
        }

        return targetPosition;
    }
}
