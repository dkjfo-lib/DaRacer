using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float dayLength = 30;

    void Start()
    {
        
    }

    void Update()
    {
        var degreesPerSecond = 360 / dayLength;
        transform.Rotate(Vector3.right, degreesPerSecond * Time.deltaTime);
    }
}
