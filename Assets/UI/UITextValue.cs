using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextValue : MonoBehaviour
{
    public ClampedValue MonitoredNumber;
    public Text text;
    public UIClampedValueDisplayMode mode;

    float localvalue = -12903;
    float localmaxvalue = -12903;

    void Start()
    {
        StartCoroutine(MonitorNumber());
    }

    IEnumerator MonitorNumber()
    {
        while (true)
        {
            yield return new WaitUntil(() => MonitoredNumber.Value != localvalue || MonitoredNumber.MaxValue != localmaxvalue);

            localvalue = MonitoredNumber.Value;
            localmaxvalue = MonitoredNumber.MaxValue;

            switch (mode)
            {
                case UIClampedValueDisplayMode.dash:
                    text.text = $"{localvalue.ToString("00")}/{localmaxvalue.ToString("00")}";
                    break;
                case UIClampedValueDisplayMode.percent:
                    text.text = $"{(localvalue * 100 / localmaxvalue).ToString("000")}%";
                    break;
                case UIClampedValueDisplayMode.raw:
                    text.text = $"{localvalue.ToString("00")}";
                    break;
                default:
                    break;
            }
        }
    }
}

public enum UIClampedValueDisplayMode
{
    dash,
    percent,
    raw
}