using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClampedValue", menuName = "Pipes/ClampedValue")]
public class ClampedValue : ScriptableObject
{
    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;
    [SerializeField] private float _value;

    public float MaxValue => maxValue;
    public float MinValue => minValue;
    public float Value 
    { 
        get => _value; 
        set => _value = Mathf.Clamp(value, minValue, maxValue); 
    }

    public void SetMaxValue(float newValue)
    {
        maxValue = newValue;
        _value = Mathf.Clamp(_value, minValue, maxValue);
    }
    public void SetMinValue(float newValue)
    {
        minValue = newValue;
        _value = Mathf.Clamp(_value, minValue, maxValue);
    }
}
