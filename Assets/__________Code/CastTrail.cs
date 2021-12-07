using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTrail : MonoBehaviour
{
    public LineRenderer lr;
    [Space]
    public float length = 1;

    Vector3 startPosition;
    float traveledDistance => (transform.position - startPosition).magnitude;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        var renderLength = traveledDistance < length ? traveledDistance : length;
        for (int i = 0; i < lr.positionCount; i++)
        {
            float pointPercent = (float)i / lr.positionCount;
            lr.SetPosition(i, transform.position - transform.forward * renderLength * pointPercent);
        }
    }
}
