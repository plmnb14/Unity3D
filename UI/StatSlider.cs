using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour
{
    Slider slider;

    public void ChangeSliderValue(float value)
    {
        if (value < 0.0f)
            value = 0.0f;

        slider.value = value;
    }

    public void SetUp(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
}


