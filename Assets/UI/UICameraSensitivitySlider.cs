using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraSensitivitySlider : MonoBehaviour
{
    public Number CameraSensitivity;
    public Slider CameraSensitivitySlider;
    float localValue;

    private void Start()
    {
        localValue = CameraSensitivity.Value;
        CameraSensitivitySlider.SetValueWithoutNotify(CameraSensitivity.Value);
    }

    public void SetNewValue(float newValue)
    {
        CameraSensitivity.Value = newValue;
    }

    void Update()
    {
        if (localValue == CameraSensitivity.Value) return;

        localValue = CameraSensitivity.Value;
        CameraSensitivitySlider.SetValueWithoutNotify(localValue);
    }
}
