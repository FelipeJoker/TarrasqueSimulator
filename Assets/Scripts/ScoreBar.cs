using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{

    public Slider slider;


    public void SetMaxScoreBar(float scoreBarValue)
    {

        slider.maxValue = scoreBarValue;

    }

    public void SetMinScoreBar(float scoreBarValue)
    {

        slider.minValue = scoreBarValue;

    }

    public void SetInitialScoreBarValue(float scoreBarValue)
    {

        slider.value = scoreBarValue;

    }



    public void SetPlayerScoreBarValue(float scoreBarValue)
    {
        slider.value = scoreBarValue;
    }



}
