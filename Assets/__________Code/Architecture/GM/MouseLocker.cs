using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    void Awake()
    {
        Lock();
    }

    public static void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
