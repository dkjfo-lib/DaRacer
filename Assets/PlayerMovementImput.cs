using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementImput : MonoBehaviour
{
    public ClampedValue MovementValueForward;
    public ClampedValue MovementValueSide;
    public ClampedValue MovementValueUp;

    public float sidewaysSens = .2f;
    public float forwardSens = .2f;
    public float forwardDump = .2f;

    private void Awake()
    {
        MovementValueSide.Value = 0;
        MovementValueUp.Value = 0;
        MovementValueForward.Value = 0;
    }

    void Update()
    {
        var input = CreateInput();
        if (input.x != 0)
        {

            MovementValueSide.Value += input.x * sidewaysSens * Time.deltaTime;
        }
        else 
        {
            var change = sidewaysSens * Time.deltaTime;
            if (MovementValueSide.Value * MovementValueSide.Value < change * change)
            {
                MovementValueSide.Value = 0;
            }
            else
            {
                MovementValueSide.Value -= Mathf.Sign(MovementValueSide.Value) * change;
            }
        }
        MovementValueUp.Value = input.y;
        if (input.z != 0)
        {

            MovementValueForward.Value += input.z * forwardSens * Time.deltaTime;
        }
        else
        {
            var change = forwardDump * Time.deltaTime;
            if (MovementValueForward.Value * MovementValueForward.Value < change * change)
            {
                MovementValueForward.Value = 0;
            }
            else
            {
                MovementValueForward.Value -= Mathf.Sign(MovementValueForward.Value) * change;
            }
        }
    }

    Vector4 CreateInput()
    {
        var input = Vector4.zero;

        if (Input.GetKey(KeyCode.A))
            input -= new Vector4(1, 0, 0, 0);
        if (Input.GetKey(KeyCode.D))
            input += new Vector4(1, 0, 0, 0);

        if (Input.GetKey(KeyCode.W))
            input += new Vector4(0, 0, 1, 0);
        if (Input.GetKey(KeyCode.S))
            input -= new Vector4(0, 0, 1, 0);

        if (Input.GetKey(KeyCode.Q))
            input -= new Vector4(0, 0, 0, 1);
        if (Input.GetKey(KeyCode.E))
            input += new Vector4(0, 0, 0, 1);

        if (Input.GetKey(KeyCode.Space))
            input += new Vector4(0, 1, 0, 0);
        if (Input.GetKey(KeyCode.LeftShift))
            input -= new Vector4(0, 1, 0, 0);

        return input;
    }
}
