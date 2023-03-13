using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthBar : MonoBehaviour
{
    public Slider slider;

    public Image sliderForColorChange;

    public void SetMaxGrowth(int growth)
    {
        slider.maxValue = growth;
        slider.value = 0;
    }

    public void SetPlayerGrowth(int growth)
    {
        slider.value = growth;
    }
}
