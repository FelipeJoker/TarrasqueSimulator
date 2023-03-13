using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreHUD : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI xText;
    public TextMeshProUGUI multiplierText;

    public GameObject boomImage;

    public float maxScoreBarThisLvl;
    public float currentScoreBar;
    public int multiplierValue = 1;

    public int scoreCalcForBar;

    public int currentScore;

    public ScoreBar scoreBar;

    public Vector2 initialBoomImageSize;

    public bool pulsing = false;

    public Vector3 boomUsualSize = new Vector3 (1,1,1);
    public Vector3 boomReducedSize = new Vector3(0.6f, 0.6f, 0.6f);
    public float timetogetsmaller;
    public float timeToGetBigger;

    public bool pulsingCoroutineIsRunning = false;

    public float pulsingThingToAdjust = 2;


    public void IncreaseScore(int score) // chamado por todos os objetos e inimigos atingidos.
    {

        if (score <=5)
        {
            scoreCalcForBar = score;
        }
        if (score > 5 && score <= 10)
        {
            scoreCalcForBar = 2;
        }
        if (score >10 && score <=20)
        {
            scoreCalcForBar = 4;
        }
        else if(score > 20 && score <=30)
        {
            scoreCalcForBar = 6;
        }
        else if (score > 30)
        {
            scoreCalcForBar = 8;
        }


        // barra multiplicadora
        if ((currentScoreBar + scoreCalcForBar) <= 13) //13, 26, 39, 52, 65
        {
            multiplierValue = 1;
            pulsingThingToAdjust = 1f;
            currentScoreBar += scoreCalcForBar;
            scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            pulsing = false;

            IncreaseTheRealScore(score);

        }
        else if ((currentScoreBar + scoreCalcForBar) > 13 && (currentScoreBar + scoreCalcForBar) <= 26)
        {
            multiplierValue = 2;
            pulsingThingToAdjust = 0.8f;
            adjustScoreBarMaxAndMin();

            xText.enabled = true;
            multiplierText.enabled = true;
            boomImage.gameObject.SetActive(true);
            pulsing = true;

            currentScoreBar += scoreCalcForBar;
            scoreBar.SetPlayerScoreBarValue(currentScoreBar);

            multiplierText.text = multiplierValue.ToString();
            if(pulsingCoroutineIsRunning == false)
            {
                StartCoroutine(PulsingMultiplier());
            }

            IncreaseTheRealScore(score);

        }
        else if ((currentScoreBar + scoreCalcForBar) > 26 && (currentScoreBar + scoreCalcForBar) <= 39)
        {
            multiplierValue = 3;
            pulsingThingToAdjust = 0.6f;
            adjustScoreBarMaxAndMin();

            xText.enabled = true;
            multiplierText.enabled = true;
            boomImage.gameObject.SetActive(true);
            pulsing = true;

            currentScoreBar += scoreCalcForBar;
            scoreBar.SetPlayerScoreBarValue(currentScoreBar);

            multiplierText.text = multiplierValue.ToString();
            if (pulsingCoroutineIsRunning == false)
            {
                StartCoroutine(PulsingMultiplier());
            }

            IncreaseTheRealScore(score);

        }
        else if ((currentScoreBar + scoreCalcForBar) > 39 && (currentScoreBar + scoreCalcForBar) <= 52)
        {
            multiplierValue = 4;
            pulsingThingToAdjust = 0.4f;
            adjustScoreBarMaxAndMin();

            xText.enabled = true;
            multiplierText.enabled = true;
            boomImage.gameObject.SetActive(true);
            pulsing = true;

            currentScoreBar += scoreCalcForBar;
            scoreBar.SetPlayerScoreBarValue(currentScoreBar);

            multiplierText.text = multiplierValue.ToString();
            if (pulsingCoroutineIsRunning == false)
            {
                StartCoroutine(PulsingMultiplier());
            }

            IncreaseTheRealScore(score);

        }
        else if ((currentScoreBar + scoreCalcForBar) > 52 && (currentScoreBar + scoreCalcForBar) <= 65)
        {
            multiplierValue = 5;
            pulsingThingToAdjust = 0.2f;
            adjustScoreBarMaxAndMin();

            xText.enabled = true;
            multiplierText.enabled = true;
            boomImage.gameObject.SetActive(true);
            pulsing = true;

            currentScoreBar += scoreCalcForBar;
            scoreBar.SetPlayerScoreBarValue(currentScoreBar);

            multiplierText.text = multiplierValue.ToString();
            if (pulsingCoroutineIsRunning == false)
            {
                StartCoroutine(PulsingMultiplier());
            }

            IncreaseTheRealScore(score);

        }
        else if ((currentScoreBar + scoreCalcForBar) > 65)
        {
            currentScoreBar = 65;

            IncreaseTheRealScore(score);
        }



    }

    void IncreaseTheRealScore(int finalscore)
    {
        // Pontos finais
        currentScore += (finalscore * multiplierValue);
        scoreText.text = currentScore.ToString("0000000000");
    }

    void adjustScoreBarMaxAndMin()
    {
        if(multiplierValue == 1)
        {
            maxScoreBarThisLvl = 13;
            scoreBar.SetMinScoreBar(0);
            scoreBar.SetMaxScoreBar(13);
        }
        if (multiplierValue == 2)
        {
            maxScoreBarThisLvl = 26;
            scoreBar.SetMinScoreBar(13);
            scoreBar.SetMaxScoreBar(26);
        }
        if (multiplierValue == 3)
        {
            maxScoreBarThisLvl = 39;
            scoreBar.SetMinScoreBar(26);
            scoreBar.SetMaxScoreBar(39);
        }
        if (multiplierValue == 4)
        {
            maxScoreBarThisLvl = 52;
            scoreBar.SetMinScoreBar(39);
            scoreBar.SetMaxScoreBar(52);
        }
        if (multiplierValue == 5)
        { 
            maxScoreBarThisLvl = 65;
            scoreBar.SetMinScoreBar(52);
            scoreBar.SetMaxScoreBar(65);
        }
        
    }


    




    IEnumerator PulsingMultiplier() // fazer a imagem aumentar e diminuir
    {
        pulsingCoroutineIsRunning = true;

        

        timetogetsmaller = 0;
        do
        {
            boomImage.GetComponent<RectTransform>().localScale = Vector3.Lerp(boomUsualSize, boomReducedSize, timetogetsmaller/ pulsingThingToAdjust);
            timetogetsmaller += Time.deltaTime;
            yield return null;
        }
        while (timetogetsmaller <= pulsingThingToAdjust); 

        timeToGetBigger = 0;
        do
        {
            boomImage.GetComponent<RectTransform>().localScale = Vector3.Lerp(boomReducedSize, boomUsualSize, timeToGetBigger/ pulsingThingToAdjust);
            timeToGetBigger += Time.deltaTime;
            yield return null;
        }
        while (timeToGetBigger <= pulsingThingToAdjust); 

        if(multiplierValue >= 2)
        {
            StartCoroutine(PulsingMultiplier());
        }



 
    }




    // Start is called before the first frame update
    void Start()
    {
        xText.enabled = false;
        multiplierText.enabled = false;
        boomImage.gameObject.SetActive(false);


        scoreBar = FindObjectOfType<ScoreBar>();


        // barra multiplicadora
        currentScoreBar = 0;
        maxScoreBarThisLvl = 13;

        scoreBar.SetPlayerScoreBarValue(currentScoreBar);
        scoreBar.SetInitialScoreBarValue(currentScoreBar);
        scoreBar.SetMaxScoreBar(maxScoreBarThisLvl);


        // Pontos finais
        currentScore = 0;
        IncreaseScore(0);

        //tamanho da explosãozinha
     


    }

    // Update is called once per frame
    void Update()
    {
        if(currentScoreBar > 0 ) // é pra sempre mudar, mas não ficar processando se < 0
        {
            if(multiplierValue ==1)
            {
                currentScoreBar -= Time.deltaTime / 3f; // reduzir a barra ao longo do tempo
                scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            }
            else if (multiplierValue == 2)
            {
                currentScoreBar -= Time.deltaTime / 2f; // reduzir a barra ao longo do tempo
                scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            }
            else if (multiplierValue == 3)
            {
                currentScoreBar -= Time.deltaTime / 1.75f; // reduzir a barra ao longo do tempo
                scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            }
            else if (multiplierValue == 4)
            {
                currentScoreBar -= Time.deltaTime / 1.5f; // reduzir a barra ao longo do tempo
                scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            }
            else if (multiplierValue == 5)
            {
                currentScoreBar -= Time.deltaTime / 1.2f; // reduzir a barra ao longo do tempo
                scoreBar.SetPlayerScoreBarValue(currentScoreBar);
            }

        }


        if (currentScoreBar < 13 )
        {
            multiplierValue = 1;
            pulsingThingToAdjust = 1f;
            adjustScoreBarMaxAndMin();

            xText.enabled = false;
            multiplierText.enabled = false;
            boomImage.gameObject.SetActive(false);
            if(pulsingCoroutineIsRunning == true)
            {
                StopCoroutine(PulsingMultiplier());
                pulsingCoroutineIsRunning = false;
            }


        }

        if (currentScoreBar >=13 && currentScoreBar <26)
        {

            multiplierValue = 2;
            pulsingThingToAdjust = 0.8f;
            adjustScoreBarMaxAndMin();
            multiplierText.text = multiplierValue.ToString();

        }

        if (currentScoreBar >= 26 && currentScoreBar < 39)
        {

            multiplierValue = 3;
            pulsingThingToAdjust = 0.6f;
            adjustScoreBarMaxAndMin();
            multiplierText.text = multiplierValue.ToString();

        }

        if (currentScoreBar >= 39 && currentScoreBar < 52)
        {

            multiplierValue = 4;
            pulsingThingToAdjust = 0.4f;
            adjustScoreBarMaxAndMin();
            multiplierText.text = multiplierValue.ToString();

        }

        if (currentScoreBar >= 52 && currentScoreBar < 65)
        {

            multiplierValue = 5;
            pulsingThingToAdjust = 0.2f;
            adjustScoreBarMaxAndMin();
            multiplierText.text = multiplierValue.ToString();

        }

               
    }


}
