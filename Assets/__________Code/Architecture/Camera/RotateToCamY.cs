using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamY : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, CamRotationY.RotationY, transform.eulerAngles.z);
    }
}
