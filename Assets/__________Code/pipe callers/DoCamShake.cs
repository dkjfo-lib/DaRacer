using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCamShake : MonoBehaviour
{
    public Pipe_CamShakes Pipe_CamShakes;
    public ShakeAtributes shake = new ShakeAtributes(.1f, .1f, 1000);
    public bool onStart = true;
    public bool onDestroy = true;

    void Start()
    {
        if (onStart)
            Pipe_CamShakes.AddCamShake(shake);
    }

    private void OnDestroy()
    {
        if (onDestroy)
            Pipe_CamShakes.AddCamShake(shake);
    }
}
