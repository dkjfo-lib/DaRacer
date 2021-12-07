using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraSensitivityField : MonoBehaviour
{
    public Number CameraSensitivity;
    public InputField InputField;
    float localValue;

    private void Start()
    {
        localValue = CameraSensitivity.Value;
        InputField.SetTextWithoutNotify(CameraSensitivity.Value.ToString());
    }

    public void OnNewValue(string newString)
    {
        int newValue;
        if (int.TryParse(newString, out newValue))
        {
            CameraSensitivity.Value = newValue;
        }
        else
        {
            InputField.SetTextWithoutNotify(CameraSensitivity.Value.ToString());
        }
    }

    void Update()
    {
        if (localValue == CameraSensitivity.Value) return;

        localValue = CameraSensitivity.Value;
        InputField.SetTextWithoutNotify(localValue.ToString());
    }
}
