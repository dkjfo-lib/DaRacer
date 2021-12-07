using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotatingValue : MonoBehaviour
{
    public ClampedValue MonitoredNumber;
    public GameObject axis;
    public float angleMaxValue = 0;
    public float angleMinValue = -180;

    float localValue = -12903;
    float localMaxValue = -12903;
    float localMinValue = -12903;

    void Start()
    {
        StartCoroutine(MonitorNumber());
    }

    IEnumerator MonitorNumber()
    {
        while (true)
        {
            yield return new WaitUntil(() => MonitoredNumber.Value != localValue || MonitoredNumber.MaxValue != localMaxValue || MonitoredNumber.MinValue != localMinValue);

            localValue = MonitoredNumber.Value;
            localMaxValue = MonitoredNumber.MaxValue;
            localMinValue = MonitoredNumber.MinValue;

            var valuePercent = (localValue - localMinValue) / (localMaxValue - localMinValue);
            var angle = Mathf.Lerp(angleMinValue, angleMaxValue, valuePercent);

            axis.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
