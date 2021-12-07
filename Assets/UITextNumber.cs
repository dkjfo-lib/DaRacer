using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextNumber : MonoBehaviour
{
    public Number number;
    public Text text;

    float localvalue = -12903;

    void Start()
    {
        StartCoroutine(MonitorNumber());
    }

    IEnumerator MonitorNumber()
    {
        while (true)
        {
            yield return new WaitUntil(() => number.Value != localvalue );

            localvalue = number.Value;

            text.text = $"{localvalue.ToString("00")}";
        }
    }
}
