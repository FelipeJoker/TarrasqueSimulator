using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Peasant : MonoBehaviour
{

    [SerializeField] float fearRange = 20f;

    [SerializeField] float turnSpeed = 10f;

    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;

    float distanceToTarget = Mathf.Infinity;

    public bool isAfraid = false;

    public Vector3 usualSize;
    public Vector3 deathSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    public float deathTimer = 10;
    public float timeForDisable = 0;
    public bool isDead = false;




    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
        usualSize = transform.localScale;
    }



    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {



            distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (isAfraid)
            {
                RunAway();

            }
            else if (distanceToTarget <= fearRange && GetComponent<Animator>().GetBool("scared") == false)
            {


                Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position));

                transform.rotation = Quaternion.LookRotation((target.position - transform.position));

                GetComponent<Animator>().SetBool("scared", true);

                GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                GetComponent<AudioSource>().Play();


            }

        }
    }

    void RunAway()
    {

        if (distanceToTarget > fearRange * 2)
        {
            ReturnToIdle();
      
        }

        else
        {
            FleeFromTarget();

        }

        

    }

    private void ReturnToIdle()
    {
        isAfraid = false;
        GetComponent<Animator>().SetTrigger("idle");
        navMeshAgent.SetDestination(transform.position);

    }

    void FleeFromTarget()
    {


            Quaternion lookRotation = Quaternion.LookRotation((transform.position - target.position));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 2);



        // Vector3 runTo = transform.position + ((transform.position - target.position).normalized *(fearRange * 0.01f)); 

        Vector3 dirToPlayer = transform.position - target.position;

        Vector3 runTo = transform.position + dirToPlayer;
        navMeshAgent.SetDestination(runTo);






    }


    public void JumpScared() // vem da animação
    {

        GetComponent<Animator>().SetBool("scared", false);
        GetComponent<Animator>().SetTrigger("run");

        isAfraid = true;


    }


    public void PeasantDie()
    {

        //gameObject.SetActive(false);
        StartDeathSequence();
    }

    public void StartDeathSequence()
    {

        isDead = true;
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().angularSpeed = 0;
        GetComponent<Animator>().SetTrigger("death");
        StartCoroutine(ProcessToDeath());
        GetComponent<BoxCollider>().isTrigger = false;

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

            transform.localScale = Vector3.Lerp(usualSize, deathSize, (timeForDisable / deathTimer));

            timeForDisable += Time.deltaTime;

            yield return null;
        }

        while (timeForDisable < deathTimer);


        gameObject.SetActive(false);
        


    }





    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fearRange);


    }

}

