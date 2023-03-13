using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{

    public int scoreValue = 10;
    public int foodValue = 1;
    public bool isInvincible = false;
    public int maxHealth = 1;
    public int currentHealth = 1;
    public bool hasAddedScoreAlready = false;
    public float timeForDisable = 0;
    public bool isDead = false;
    public NavMeshAgent thisNavMesh;
    public Vector3 usualSize;
    public Vector3 deathSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    public float deathTimer = 10;
    public float usualSpeed;
    public float usualAngularSpeed;


    public ScoreHUD referToScoreHUD;


    void Start()
    {
        currentHealth = maxHealth;
        referToScoreHUD = FindObjectOfType<ScoreHUD>();
        thisNavMesh = GetComponent<NavMeshAgent>();
        usualSize = transform.localScale;
    }




    public void TakeDamage(int damage)
    {
        if (isInvincible == false)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                StartDeathSequence();
            }


        }

    }

    public void StartDeathSequence()
    {
        if(GetComponent<Chicken>())
        {
            isDead = true;
            referToScoreHUD.IncreaseScore(scoreValue);
            hasAddedScoreAlready = true;
            GetComponent<Chicken>().ChickenDie();
        }
        else if (GetComponent<Frog>())
        {
            isDead = true;
            referToScoreHUD.IncreaseScore(scoreValue);
            hasAddedScoreAlready = true;
            GetComponent<Frog>().ChickenDie();
        }
        else if (GetComponent<Peasant>())
        {
            isDead = true;
            referToScoreHUD.IncreaseScore(scoreValue);
            hasAddedScoreAlready = true;
            GetComponent<Peasant>().PeasantDie();
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = false;
            isDead = true;
            thisNavMesh.speed = 0;
            thisNavMesh.angularSpeed = 0;
            GetComponent<Animator>().SetBool("IsAttacking", false);
            GetComponent<Animator>().SetBool("IsWalking", false);
            GetComponent<Animator>().SetTrigger("death");
            hasAddedScoreAlready = true;
            referToScoreHUD.IncreaseScore(scoreValue);

            StartCoroutine(ProcessToDeath());

        }

    }


    IEnumerator ProcessToDeath()
    {

        timeForDisable = 0;

        do
        {
            timeForDisable += Time.deltaTime;
            yield return null;
        }

        while (timeForDisable < 5);


        timeForDisable = 0;

        do
        {

            transform.localScale = Vector3.Lerp(usualSize, deathSize, timeForDisable / deathTimer);

            timeForDisable += Time.deltaTime;

            yield return null;
        }

        while (timeForDisable < deathTimer);


        gameObject.SetActive(false);
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        currentHealth = maxHealth;
        thisNavMesh.speed = usualSpeed;
        thisNavMesh.angularSpeed = usualAngularSpeed;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.localScale = usualSize;
        isDead = false;



    }


}
