using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotationX : MonoBehaviour
{
    public static float RotationX { get; private set; } = 0;

    public Number CameraSensitivity;
    float rotationSpeed => CameraSensitivity.Value;
    public float limitAngle = 80;

    void FixedUpdate()
    {
        float addAngleX = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
        
        float newAngleX;
        if (transform.localRotation.eulerAngles.x > 180)
            newAngleX = Mathf.Clamp(transform.localRotation.eulerAngles.x - 360 + addAngleX, -limitAngle, limitAngle);
        else
            newAngleX = Mathf.Clamp(transform.localRotation.eulerAngles.x + addAngleX, -limitAngle, limitAngle);

        RotationX = newAngleX;
        transform.localRotation = Quaternion.Euler(newAngleX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
