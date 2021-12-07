using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGUI : MonoBehaviour
{
    public Canvas canvas;
    public Slider healthSlider;

    public void Init(int maxHealth, int health)
    {
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        SetHP(health);
    }

    public void UpdateUI(int health, bool isDisplay)
    {
        SetHP(health);
        DisplayGUI(isDisplay);
    }

    void SetHP(int hp)
    {
        if (healthSlider == null) return;
        healthSlider.value = hp;
    }

    void DisplayGUI(bool isDisplay)
    {
        if (canvas.gameObject.activeSelf != isDisplay)
            canvas.gameObject.SetActive(isDisplay);
    }
}
