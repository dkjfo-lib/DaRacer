using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UpdateShadersSunDirection : MonoBehaviour
{
    public Material[] materials;

    void Update()
    {
        foreach (var mat in materials)
        {
            mat.SetVector("SunForward", transform.forward);
        }
    }
}
