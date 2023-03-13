using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CasterEnemy : MonoBehaviour
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
    public int fireballDMG = 0;


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

    public bool isCaster = false;
    public int randomCasterAction;
    public bool isCasting = false;

    public GameObject priestUolalo;
    public Transform overHeadTarget;

    public bool hasStartAttackSound = false;
    public AudioClip startAttackSound;
    public AudioClip uolaloSound;
    public AudioClip shieldAllySound;
    public GameObject allyTarget;
    private float distanceToAlly;
    public GameObject shieldEffect;
    public bool isInvincible = false;
    public ParticleSystem anotherParticleEffect;

    public bool isWizard = false;
    public ParticleSystem magicMissiles;
    public ParticleSystem lightningBolt;
    public ParticleSystem lightningBoltCharge;
    public ParticleSystem fireBall;
    public ParticleSystem fireballCharge;
    public ParticleSystem meteorCharge;
    public bool activateFireBallDamage = false;

    public GameObject meteorShower;
    public AudioClip magicMissileSFX;
    public AudioClip lightningSFX;
    public AudioClip fireballSFX;
    public AudioClip fireballImpactSFX;
    public AudioClip meteorPortalSFX;
    public AudioClip meteorFallSFX;
    public AudioClip meteorImpactSFX;
    public AudioClip fireBallChargeSFX;
    public AudioClip meteorChargeSFX;
    public GameObject rangedMagicStartingPoint;

    public GameObject lighteningFINAL;
    public GameObject magicMissileFINAL;

    public bool doesNotMove = false;

    public int scoreValue = 10;
    public ScoreHUD referToScoreHUD;
    public bool hasAddedScoreAlready = false;

    Vector3 _direction;
    Quaternion particleSystemRotationMaster;


    // Start is called before the first frame update
    void Start()
    {


        usualSpeed = GetComponent<NavMeshAgent>().speed;
        usualAngularSpeed = GetComponent<NavMeshAgent>().angularSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();


        target = FindObjectOfType<PlayerMovement>().transform;
        overHeadTarget = FindObjectOfType<OverHeadTarget>().transform;
        rangedAttackTarget = FindObjectOfType<RangedAttackTarget>().transform;
        usualSize = transform.localScale;

        navMeshAgent.stoppingDistance = target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1);
        currentHealth = maxHealth;

        referToScoreHUD = FindObjectOfType<ScoreHUD>();


    }



    // Update is called once per frame
    void Update()
    {



        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (allyTarget != null)
        {
            distanceToAlly = Vector3.Distance(allyTarget.transform.position, transform.position);
        }

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
                else if (distanceToTarget <= chaseRange && isCasting == false)
                {
                    isProvoked = true;

                }

                if (distanceToAlly >= chaseRange)
                {
                    ReturnToIdleMode();
                }





            }

        }

        if (doesNotMove == true && isRanged == false) // melee plant
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

    }

    private void ReturnToIdleMode()
    {
        navMeshAgent.SetDestination(transform.position);
        isProvoked = false;
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<Animator>().SetTrigger("idle");



        if (allyTarget != null)
        {
            allyTarget.GetComponent<Warrior>().RemoveInvincibility();

        }

        if (anotherParticleEffect != null)
        {
            anotherParticleEffect.Stop();
        }

        if (magicMissiles != null)
        {
            magicMissiles.Stop();
        }
        if (lightningBolt != null)
        {
            lightningBolt.Stop();
        }
        if (fireBall != null)
        {
            fireBall.Stop();
        }

        if (rangedAttackParticleSystem != null)
        {
            rangedAttackParticleSystem.Stop();

        }

        if (isCaster && isCasting == true)
        {
            isCasting = false;
        }

    }

    void EngageTarget() // if is provoked (if player got close)
    {
        FaceTargetWhenAttacking();

        if (isRanged == true)
        {
            AimWhenAttackingRanged();
        }

        if (isRanged == false && isCaster == false) // warrior
        {
            if (distanceToTarget > target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1))
            {
                ChaseTarget();
            }

        }
        else if (isRanged == true && isCaster == false) // archer
        {
            if (distanceToTarget > (target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                ChaseTarget();
            }
        }
        else if (isRanged == false && isCaster == true && isCasting == false) // priest & Wizard
        {
            if (distanceToTarget > (target.GetComponent<PlayerHealth>().sizeCombatRadius + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                ChaseTarget();
            }
        }


        if (isRanged == false && isCaster == false) // warrior
        {

            if (distanceToTarget <= target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1))
            {
                StartAttackTarget();
            }

        }
        else if (isRanged == true && isCaster == false) // archer
        {
            if (distanceToTarget <= (target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                StartRangedAttack();
            }

        }
        else if (isRanged == false && isCaster == true && isWizard == false && isCasting == false) //priest
        {
            if (distanceToTarget <= (target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                if (isCasting == false)
                {
                    isCasting = true;
                    PriestRandomAction();
                }

            }

        }
        else if (isRanged == false && isCaster == true && isWizard == true && isCasting == false) //Wizard
        {
            if (distanceToTarget <= (target.GetComponent<PlayerHealth>().sizeCombatRadius + 0.2f + (enemySizeForDistance - 1) + rangedAttackRange))
            {
                if (isCasting == false)
                {
                    isCasting = true;
                    WizardRandomAction();
                }

            }

        }










    }
    //-------------------------------------------WIZARD---------------------------------------------------------

    void WizardRandomAction()
    {
        randomCasterAction = UnityEngine.Random.Range(1, 11);

        if (randomCasterAction <= 3)
        {
            WizardMagicMissileStart();
        }
        else if (randomCasterAction > 3 && randomCasterAction <= 6)
        {
            WizardLightningBoltStart();
        }
        else if (randomCasterAction > 6 && randomCasterAction <= 9)
        {
            WizardFireBallStart();
        }
        else if (randomCasterAction > 9)
        {
            WizardMeteorStart();
        }

    }

    void WizardMagicMissileStart()
    {
        isCasting = true;
        FaceTargetWhenAttacking();
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", true);

        _direction = (rangedAttackTarget.position - rangedMagicStartingPoint.transform.position);
        particleSystemRotationMaster.eulerAngles = _direction;
        magicMissiles.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().PlayOneShot(magicMissileSFX);

        StartCoroutine(WizardActionEnd(5));

    }

    void WizardLightningBoltStart()
    {
        isCasting = true;
        FaceTargetWhenAttacking();
        AimWhenAttackingMagic();
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", true);

        lightningBoltCharge.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.7f;
        GetComponent<AudioSource>().PlayOneShot(lightningSFX);

        StartCoroutine(WizardLightningBoltSequence());


    }

    void WizardFireBallStart()
    {
        isCasting = true;
        activateFireBallDamage = true;
        FaceTargetWhenAttacking();
        AimWhenAttackingMagic();
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", true);

        fireballCharge.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().PlayOneShot(fireBallChargeSFX);

        StartCoroutine(WizardFireBallSequence());


    }

    void WizardMeteorStart()
    {
        isCasting = true;
        activateFireBallDamage = true;
        FaceTargetWhenAttacking();
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetTrigger("meteorShower");
        GetComponent<Animator>().SetBool("IsWalking", false);
        meteorCharge.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().PlayOneShot(meteorChargeSFX);

        StartCoroutine(WizardMeteorShowerSequence());


    }

    IEnumerator WizardMeteorShowerSequence()
    {


        float timeTillAttack = 0;

        do
        {
            timeTillAttack += Time.deltaTime;
            yield return null;
        }
        while (timeTillAttack < 7f);

        meteorCharge.Stop();


        GameObject _meteorShowerInstance = (GameObject)Instantiate(meteorShower, (overHeadTarget.position + new Vector3(0, 15, 0)), Quaternion.identity, this.gameObject.transform);
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().PlayOneShot(meteorPortalSFX);

        Destroy(_meteorShowerInstance, 5f);
        StartCoroutine(WizardActionEnd(7));



    }



    IEnumerator WizardFireBallSequence()
    {

        float timeTillAttack = 0;

        do
        {
            timeTillAttack += Time.deltaTime;
            yield return null;
        }
        while (timeTillAttack < 2.8f);


        _direction = (rangedAttackTarget.position - rangedMagicStartingPoint.transform.position);
        particleSystemRotationMaster.eulerAngles = _direction;
        Quaternion lookRotation = Quaternion.LookRotation(_direction, new Vector3(0, 10, 0));
        rangedMagicStartingPoint.transform.rotation = lookRotation;

        fireBall.transform.rotation = particleSystemRotationMaster;
        fireBall.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().PlayOneShot(fireballSFX);
        StartCoroutine(WizardActionEnd(3));

    }


    IEnumerator WizardLightningBoltSequence()
    {
        float timeTillAttack = 0;

        do
        {
            timeTillAttack += Time.deltaTime;
            yield return null;
        }
        while (timeTillAttack < 2.5f);
        FaceTargetWhenAttacking();

        _direction = (rangedAttackTarget.position - rangedMagicStartingPoint.transform.position);
        particleSystemRotationMaster.eulerAngles = _direction;
        GameObject LBFINAL = Instantiate(lighteningFINAL, rangedMagicStartingPoint.transform.position, Quaternion.LookRotation(_direction), transform);

        Destroy(LBFINAL, 4);
        StartCoroutine(WizardActionEnd(4));

    }



    IEnumerator WizardActionEnd(float endTimer)
    {

        float timeToEndWizardAttack = 0;

        do
        {
            timeToEndWizardAttack += Time.deltaTime;
            yield return null;
        }

        while (timeToEndWizardAttack < endTimer);

        activateFireBallDamage = false;
        ReturnToIdleMode();

    }



    //-------------------------------------------END WIZARD---------------------------------------------------------




    //-------------------------------------------PRIEST---------------------------------------------------------


    void PriestRandomAction()
    {

        randomCasterAction = 2;   //UnityEngine.Random.Range(1, 10);

        if (randomCasterAction <= 3)
        {
            PriestAttackStart();

        }
        else if (randomCasterAction > 3 && randomCasterAction <= 6)
        {
            Uolalo();
        }
        else if (randomCasterAction > 6)
        {
            PriestShieldAlly();
        }

    }

    void PriestAttackStart()
    {
        isCasting = true;
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", true);

        Vector3 direction = (rangedAttackTarget.position - rangedAttackStartingPoint.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction, new Vector3(0, 10, 0));

        rangedAttackParticleSystem.transform.rotation = lookRotation;

        FaceTargetWhenAttacking();







        rangedAttackParticleSystem.Play();
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.6f;
        GetComponent<AudioSource>().Play();

        StartCoroutine(PriestAttackEnd());

    }

    IEnumerator PriestAttackEnd()
    {

        float timeToEndPriestAttack = 0;

        do
        {
            timeToEndPriestAttack += Time.deltaTime;
            yield return null;
        }

        while (timeToEndPriestAttack < 10);

        ReturnToIdleMode();

    }

    void Uolalo()
    {
        isCasting = true;
        FaceTargetWhenAttacking();
        navMeshAgent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<Animator>().SetTrigger("wololo");
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().volume = 0.8f;
        GetComponent<AudioSource>().PlayOneShot(uolaloSound);
        GameObject uolaloEffect = (GameObject)Instantiate(priestUolalo, overHeadTarget);
        StartCoroutine(PriestAttackEnd());
        Destroy(uolaloEffect, 10f);
    }

    void PriestShieldAlly()
    {

        isCasting = true;
        navMeshAgent.SetDestination(transform.position);

        Collider[] allies = Physics.OverlapSphere(transform.position, rangedAttackRange, (1 << 7));

        if (allies.Length > 1) // se tiver mais alguem alem dele mesmo...
        {
            GetComponent<Animator>().SetBool("IsWalking", false);
            GetComponent<Animator>().SetBool("IsAttacking", false);
            GetComponent<Animator>().SetTrigger("shieldAlly");
            anotherParticleEffect.Play();
            GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().volume = 1f;
            GetComponent<AudioSource>().PlayOneShot(shieldAllySound);


            foreach (Collider ally in allies)
            {

                if (allyTarget == null && ally.CompareTag("Human") && ally.GetComponent<Warrior>().isDead == false)
                {
                    allyTarget = ally.gameObject;
                    Debug.Log(allyTarget.name);
                    GameObject shield = (GameObject)Instantiate(shieldEffect, allyTarget.transform);
                    allyTarget.GetComponent<Warrior>().Invincibility();
                }


            }


        }
        else
        {
            PriestRandomAction();
        }


    }

    public void Invincibility()
    {
        isInvincible = true;
    }

    public void RemoveInvincibility()
    {
        if (isInvincible)
        {
            isInvincible = false;
            GameObject shieldObject = GetComponentInChildren<PriestShield>().gameObject;
            Destroy(shieldObject);

        }


    }




    // ----------------------------------------END PRIEST-----------------------------------------

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

    void AimWhenAttackingMagic()
    {


        particleSystemRotationMaster = rangedMagicStartingPoint.transform.rotation;

        _direction = particleSystemRotationMaster.eulerAngles;


        Quaternion lookRotation = Quaternion.LookRotation(_direction, new Vector3(0, 10, 0));

        rangedMagicStartingPoint.transform.rotation = lookRotation;





    }



    public void TakeDamage(int damage)
    {
        if (isInvincible == false)
        {
            currentHealth -= damage;
            if (anotherParticleEffect != null)
            {
                anotherParticleEffect.Stop();

            }
            if (rangedAttackParticleSystem != null)
            {
                rangedAttackParticleSystem.Stop();

            }

            if (allyTarget != null)
            {
                allyTarget.GetComponent<Warrior>().RemoveInvincibility();
            }

            if (currentHealth <= 0)
            {
                StartDeathSequence();
            }


        }

    }



    public void StartDeathSequence()
    {

        GetComponent<BoxCollider>().isTrigger = false;
        isDead = true;
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().angularSpeed = 0;
        GetComponent<Animator>().SetBool("IsAttacking", false);
        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<Animator>().SetTrigger("death");
        hasAddedScoreAlready = true;
        referToScoreHUD.IncreaseScore(scoreValue);

        StartCoroutine(ProcessToDeath());

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
        GetComponent<NavMeshAgent>().speed = usualSpeed;
        GetComponent<NavMeshAgent>().angularSpeed = usualAngularSpeed;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.localScale = usualSize;
        isDead = false;



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

