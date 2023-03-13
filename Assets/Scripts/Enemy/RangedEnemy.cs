using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RangedEnemy : MonoBehaviour
{

    [SerializeField] float chaseRange = 5f;

    [SerializeField] float turnSpeed = 5f;

    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;

    public float distanceToTarget = Mathf.Infinity;

    public bool isProvoked = false;

    public float timeForDisable = 0;
    public bool isDead = false;

    public int maxHealth = 1;
    public int currentHealth = 1;
    public int enemyDamage = 1;

    public int enemySizeForDistance = 1;
    public int enemySizeForPlayerComparison = 1;

    public float usualSpeed;
    public float usualAngularSpeed;

    public bool isScaredFromGrowth = false;

    public bool isRanged = false;
    public float rangedAttackRange = 20;
    public Transform rangedAttackStartingPoint;
    public ParticleSystem rangedAttackParticleSystem;
    public Transform rangedAttackTarget;

    public Vector3 usualSize;
    public Vector3 deathSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    public float deathTimer = 10;


    public bool hasStartAttackSound = false;
    public AudioClip startAttackSound;
    
    public bool doesNotMove = false;
    
    Vector3 _direction;
    Quaternion particleSystemRotationMaster;


    // Start is called before the first frame update
    void Start()
    {
        
        usualSpeed = GetComponent<NavMeshAgent>().speed;
        usualAngularSpeed = GetComponent<NavMeshAgent>().angularSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();

        target = FindObjectOfType<PlayerMovement>().transform;
        rangedAttackTarget = FindObjectOfType<RangedAttackTarget>().transform;
        usualSize = transform.localScale;

        navMeshAgent.stoppingDistance = target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1);
        currentHealth = maxHealth;

    }



    // Update is called once per frame
    void Update()
    {

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isDead == false && doesNotMove == false)
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

        if (doesNotMove == true && isRanged == true) // Ranged plant
        {
            if (distanceToTarget <= rangedAttackRange)
            {
                FaceTargetWhenAttacking();
                AimWhenAttackingRanged();
                StartRangedAttack();
            }
            else
            {
                GetComponent<Animator>().SetBool("IsAttacking", false);
                GetComponent<Animator>().SetBool("IsWalking", false);
                GetComponent<Animator>().SetTrigger("idle");

            }


        }

    } // Update End

    private void ReturnToIdleMode()
    {
        navMeshAgent.SetDestination(transform.position);
        isProvoked = false;
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<Animator>().SetTrigger("idle");


        if (rangedAttackParticleSystem != null)
        {
            rangedAttackParticleSystem.Stop();
        }
        
    }

    void EngageTarget() // if is provoked (if player got close)
    {
        FaceTargetWhenAttacking();
         AimWhenAttackingRanged();

            if (distanceToTarget > (target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                ChaseTarget();
            }

            if (distanceToTarget <= (target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                StartRangedAttack();
            }

    }
   

    private void StartRangedAttack()
    {
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", true);
        GetComponent<Animator>().SetBool("IsWalking", false);

    }

    public void FireRangedAttack() // called from animation
    {
        rangedAttackParticleSystem.Play();

        if (GetComponent<AudioSource>())
        {

            GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().volume = 0.6f;
            GetComponent<AudioSource>().Play();

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

    void AimWhenAttackingRanged()
    {
        Vector3 direction = (rangedAttackTarget.position - rangedAttackStartingPoint.position);

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Quaternion particleSystemRotation = rangedAttackParticleSystem.transform.rotation;
        particleSystemRotation.eulerAngles = direction;



        rangedAttackParticleSystem.transform.rotation = lookRotation;

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
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);


    }

}

