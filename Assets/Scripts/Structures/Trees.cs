using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{


    public int currentHealth = 5;

    private float timeForDisable;

    public AudioSource audioSource;

    public ScoreHUD referToScoreHUD;
    public int scoreValue = 2;

    public bool isDestroyed = false;


    public void TreeTakeDamage(int damage)
    {
        currentHealth -= damage;

       
        if (currentHealth <= 0 && isDestroyed == false)
        {
            isDestroyed = true;
            referToScoreHUD.IncreaseScore(scoreValue);

            if (audioSource != null)
            {
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.Play();
            }

            if(GetComponent<Rigidbody>() == null)
            {
                 gameObject.AddComponent<Rigidbody>();   
            }

            StartCoroutine(ProcessToDestruction());



        }

    }

    IEnumerator ProcessToDestruction()
    {

        timeForDisable = 0;

        do
        {
            timeForDisable += Time.deltaTime;
            yield return null;
        }

        while (timeForDisable < 10);

        GetComponent<CapsuleCollider>().enabled = false;

        Destroy(gameObject, 5f);


    }


    // Start is called before the first frame update
    void Start()
    {
        referToScoreHUD = FindObjectOfType<ScoreHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
