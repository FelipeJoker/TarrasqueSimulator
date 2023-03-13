using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Chicken : MonoBehaviour
{

    [SerializeField] float fearRange = 20f;

    [SerializeField] float turnSpeed = 10f;

    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;

    float distanceToTarget = Mathf.Infinity;

    public bool isAfraid = false;

    public GameObject featherFX;




    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
    }



    // Update is called once per frame
    void Update()
    {

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isAfraid)
        {
            RunAway();

        }
        else if (distanceToTarget <= fearRange && GetComponent<Animator>().GetBool("scared") == false)
        {


            Vector3 direction = (target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation((transform.position - target.position));

            transform.rotation = Quaternion.LookRotation((transform.position - target.position));

            GetComponent<Animator>().SetBool("scared", true);

            GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();


        }

        
    }

    void RunAway()
    {

        if (distanceToTarget > fearRange*2)
        {
            ReturnToIdle();
      
        }

        else
        {
        FaceAwayWhenFleeing();
            FleeFromTarget();

        }

        

    }

    private void ReturnToIdle()
    {
        isAfraid = false;
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetTrigger("eat");


    }

    void FleeFromTarget()
    {


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


    void FaceAwayWhenFleeing()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);


    }

    public void ChickenDie()
    {

        Instantiate(featherFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fearRange);


    }

}

