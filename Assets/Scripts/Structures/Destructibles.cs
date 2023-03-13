using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructibles : MonoBehaviour
{

    public bool staysWhole = true;
    public bool hasParts = false;
    public bool hasDestroyedVersion = false;

    public GameObject destroyedVersion;

    public int currentHealth = 5;

    public AudioSource audioSource;

    private float timeForDisable;

    public int scoreValue = 1;

    public ScoreHUD referToScoreHUD;

    public void PropTakesDamage(int damage)
    {
        currentHealth -= damage;


        if (currentHealth <= 0)
        {

            referToScoreHUD.IncreaseScore(scoreValue);

            if(staysWhole == true)
            {

                if (GetComponent<Rigidbody>() == null)
                {
                    gameObject.AddComponent<Rigidbody>();
                }

                StartCoroutine(ProcessToDestruction());

                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.Play();

            }

            if (hasParts == true)
            {

                if (audioSource != null)
                {
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.Play();
                }


                foreach (Transform child in transform)
                {
                    if(GetComponent<BoxCollider>() != null)
                    {
                    GetComponent<BoxCollider>().enabled = false;
                    }




                    if (child.GetComponentInChildren<Rigidbody>() == false)
                    {
                        child.gameObject.AddComponent<Rigidbody>();
                    }



                }

                StartCoroutine(ProcessToDestructionMultipleParts());

            }


            if( hasDestroyedVersion == true)
            {
                GameObject destroyed = (GameObject)Instantiate(destroyedVersion, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                Destroy(destroyed, 10f);

            }


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

        if (GetComponent<CapsuleCollider>())
        {
        GetComponent<CapsuleCollider>().enabled = false;
        }
        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
        }


        Destroy(gameObject, 10f);


    }


    IEnumerator ProcessToDestructionMultipleParts()
    {

        timeForDisable = 0;

        do
        {
            timeForDisable += Time.deltaTime;
            yield return null;
        }

        while (timeForDisable < 10);

        if (GetComponent<CapsuleCollider>())
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<BoxCollider>())
            {
                child.gameObject.GetComponent<BoxCollider>().enabled = false;
            }

            if (child.gameObject.GetComponent<CapsuleCollider>())
            {
                child.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<BoxCollider>())
                {
                    grandchild.gameObject.GetComponent<BoxCollider>().enabled = false;
                }

                if (grandchild.gameObject.GetComponent<CapsuleCollider>())
                {
                    grandchild.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                }

            }

            Destroy(gameObject, 10f);

        }
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
