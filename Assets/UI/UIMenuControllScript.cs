using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuControllScript : MonoBehaviour
{
    public GameObject Menu;
    [Space]
    bool isMenuOpened;

    private void Start()
    {
        isMenuOpened = Menu.activeSelf;
        if (isMenuOpened)
        {
            MouseLocker.Unlock();
            Time.timeScale = 0;
        }
        else
        {
            MouseLocker.Lock();
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpened) CloseMenu();
            else OpenMenu();
        }
    }

    public void OpenMenu()
    {
        isMenuOpened = true;
        Menu.SetActive(true);
        MouseLocker.Unlock();
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        isMenuOpened = false;
        Menu.SetActive(false);
        MouseLocker.Lock();
        Time.timeScale = 1;
    }
}
