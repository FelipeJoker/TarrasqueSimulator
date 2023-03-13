using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MeleeEnemy : MonoBehaviour
{

    [SerializeField] float chaseRange = 5f;

    [SerializeField] float turnSpeed = 5f;

    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;

    public float distanceToTarget = Mathf.Infinity;

    public bool isProvoked = false;


    public int enemyDamage = 1;
    public int enemySizeForDistance = 1;
    public int enemySizeForPlayerComparison = 1;

    public float usualSpeed;
    public float usualAngularSpeed;

    public bool isScaredFromGrowth = false;

    public bool hasStartAttackSound = false;
    public AudioClip startAttackSound;
    public bool doesNotMove = false;

    public Transform rangedAttackTarget;



    // Start is called before the first frame update
    void Start()
    {
        usualSpeed = GetComponent<NavMeshAgent>().speed;
        usualAngularSpeed = GetComponent<NavMeshAgent>().angularSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
        navMeshAgent.stoppingDistance = target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1);
        rangedAttackTarget = FindObjectOfType<RangedAttackTarget>().transform; // para olhar para o peito do tarrasque quando atacar
    }



    // Update is called once per frame
    void Update()
    {

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (GetComponent<EnemyStats>().isDead == false && doesNotMove == false)
        {

            if (isScaredFromGrowth == true && distanceToTarget > chaseRange * 1.3f)
            {
                navMeshAgent.SetDestination(transform.position);
                isProvoked = false;
                isScaredFromGrowth = false;
                GetComponent<Animator>().SetBool("IsAttacking", false);
                GetComponent<Animator>().SetBool("IsWalking", false);
                GetComponent<Animator>().SetTrigger("idle");

            }

            if (isScaredFromGrowth == false)
            {

                if (distanceToTarget > chaseRange)
                {
                    ReturnToIdleMode();

                }
                else if (isProvoked)
                {
                    EngageTarget();

                }
                else if (distanceToTarget <= chaseRange)
                {
                    isProvoked = true;

                }


            }

        }

        if (doesNotMove == true) // melee plant
        {
            if (distanceToTarget <= chaseRange)
            {
                FaceTargetWhenAttacking();
                StartAttackTarget();
            }
            else
            {
                GetComponent<Animator>().SetBool("IsAttacking", false);
                GetComponent<Animator>().SetBool("IsWalking", false);
                GetComponent<Animator>().SetTrigger("idle");

            }

        }

      

    }//  Update end

    private void ReturnToIdleMode()
    {
        navMeshAgent.SetDestination(transform.position);
        isProvoked = false;
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<Animator>().SetTrigger("idle");

    }

    void EngageTarget() // if is provoked (if player got close)
    {
            FaceTargetWhenAttacking();

            if (distanceToTarget > target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1))
            {
                ChaseTarget();
            }

            if (distanceToTarget <= target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1))
            {
                StartAttackTarget();
            }

    }


    void ChaseTarget()
    {

        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", true);
        navMeshAgent.SetDestination(target.position);

    }

    void StartAttackTarget()
    {
        GetComponent<Animator>().SetBool("IsAttacking", true);
        GetComponent<Animator>().SetBool("IsWalking", false);

        if (hasStartAttackSound && GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().PlayOneShot(startAttackSound);
        }

    }

    public void EndAttackTarget() // comes from animation
    {
        if (distanceToTarget <= target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.3f + (enemySizeForDistance - 1))
        {
            if (GetComponent<AudioSource>())
            {

                GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                GetComponent<AudioSource>().Play();

            }
            target.GetComponent<PlayerHealth>().PlayerTakeDamage(enemyDamage);


        }
    }


    void FaceTargetWhenAttacking()
    {
        Vector3 direction = (rangedAttackTarget.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

    }


    // when player grows:

    public void AdjustStoppingDistance()
    {
        navMeshAgent.stoppingDistance = target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1);

    }

    public void GetScared()
    {
        if (doesNotMove == false)
        {

            isProvoked = false;
            isScaredFromGrowth = true;
            GetComponent<Animator>().SetBool("IsAttacking", false);
            GetComponent<Animator>().SetBool("IsWalking", true);

            Vector3 direction = (target.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 2);

            Vector3 runTo = transform.position + ((transform.position - target.position).normalized * (30));
            navMeshAgent.SetDestination(runTo);

        }

    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);


    }

}

