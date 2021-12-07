using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Number", menuName = "MyConsts/Number")]
public class Number : ScriptableObject
{
    [SerializeField] float _value;
    public float Value { get => _value; set => _value = value; }
}
