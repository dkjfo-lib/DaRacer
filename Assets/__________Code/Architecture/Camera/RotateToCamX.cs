using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamX : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(CamRotationX.RotationX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
