using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;


public class PlayerHealth : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;
    public float cameraOffsetAdjustToSize = 1.1f;

    public int maxPlayerHealth = 10;
    public int currentPlayerHealth;

    public int maxPlayerGrowth = 5;
    public int currentPlayerGrowth = 0;

    public int regenRate = 1;
    public float regenCooldown = 1;

    public HealthBar healthBar;
    public GrowthBar growthBar;
    public RectTransform healthBarSize;
    public float initHealthBarSize;
    public float healthBarAdjust = 150;

    public Animator animator;

    public float sizeCombatRadius = 2;

    public bool isMaxSize = false;
    public float timerForGrowth = 0f;
    public float growTime = 3f;
    public float startSize = 1;
    public float maxSize = 1.5f;
    public ParticleSystem healthIncreaseEffect;

    public AudioSource audioSource;
    public AudioClip roar;
    public AudioClip death;

    public int masterSizeValue = 1;

    public TextMeshProUGUI sizeTextUI;


    public GameObject mesh;
    public Material startMaterial;
    public Material damageMaterial;

    public float timeToChangeColor;

    public GameObject gameOverPanel;


    private void Start()
    {
        currentPlayerGrowth = 0;
        currentPlayerHealth = maxPlayerHealth;
        healthBar.SetMaxHealth(maxPlayerHealth);
        healthBar.SetInitialHealth(maxPlayerHealth);
        growthBar.SetMaxGrowth(maxPlayerGrowth);
        growthBar.SetPlayerGrowth(currentPlayerGrowth);
        initHealthBarSize = healthBarSize.sizeDelta.x;
        startMaterial = mesh.GetComponent<SkinnedMeshRenderer>().material;
        gameOverPanel = FindObjectOfType<GameOverPanel>(true).gameObject;
    }

    IEnumerator StartGameOverSequence()
    {
        float timeTillGameOver = 0;

        do
        {
            timeTillGameOver += Time.deltaTime;
            yield return null;
        }
        while (timeTillGameOver < 3f);

        gameOverPanel.SetActive(true);


    }

    public void PlayerTakeDamage(int damage)
    {
        currentPlayerHealth -= damage;
        healthBar.SetPlayerHealth(currentPlayerHealth);

        Debug.Log(damage);

        if (currentPlayerHealth >0)
        {
            StartCoroutine(ColorFlicker());
        }



        if (currentPlayerHealth <=0 && GetComponent<PlayerMovement>().controlsDisabled == false)
        {
            PlayerIsKilled();
        }
    }

    IEnumerator ColorFlicker()
    {
        mesh.GetComponent<SkinnedMeshRenderer>().material = damageMaterial;
        timeToChangeColor = 0;

        do
        {
            timeToChangeColor += Time.deltaTime;
            yield return null;
        }

        while (timeToChangeColor < 0.07f);

        mesh.GetComponent<SkinnedMeshRenderer>().material = startMaterial;

        timeToChangeColor = 0;

        do
        {
            timeToChangeColor += Time.deltaTime;
            yield return null;
        }

        while (timeToChangeColor < 0.07f);

        mesh.GetComponent<SkinnedMeshRenderer>().material = damageMaterial;

        timeToChangeColor = 0;

        do
        {
            timeToChangeColor += Time.deltaTime;
            yield return null;
        }

        while (timeToChangeColor < 0.07f);

        mesh.GetComponent<SkinnedMeshRenderer>().material = startMaterial;

    }

    private void PlayerIsKilled()
    {
        GetComponent<PlayerMovement>().controlsDisabled = true;
        animator.SetTrigger("die");
        audioSource.PlayOneShot(death);
        StartCoroutine(StartGameOverSequence());


    }

    public void PlayerIncreaseGrowth(int growth)
    {
        if(currentPlayerGrowth < maxPlayerGrowth)
        {
            currentPlayerGrowth += growth;
            growthBar.SetPlayerGrowth(currentPlayerGrowth);

        }
        else
        {
            if (masterSizeValue < 4.5f)
            {
                StartCoroutine(IncreaseSize());


            }
        }

    }



    private IEnumerator IncreaseSize()
    {
        Vector3 startScale = new Vector3(startSize, startSize, startSize);
        Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);


        if(isMaxSize == false)
        {
            GetComponent<PlayerMovement>().controlsDisabled = true;
            currentPlayerGrowth = 0;
            masterSizeValue++;
            growthBar.SetPlayerGrowth(currentPlayerGrowth);
            maxPlayerGrowth = maxPlayerGrowth + 5;
            growthBar.SetMaxGrowth(maxPlayerGrowth);
            sizeTextUI.text = masterSizeValue.ToString();

            if (masterSizeValue == 3)
            {
                GetComponent<PlayerMovement>().FootsCollidersOn();
            }


            if (masterSizeValue == 5)
            {
                sizeTextUI.text = "MAX";
                currentPlayerGrowth = maxPlayerGrowth;
                growthBar.sliderForColorChange.color = Color.white;
                growthBar.SetPlayerGrowth(currentPlayerGrowth);
            }

            animator.SetTrigger("roar");
            healthIncreaseEffect.Play();
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(roar);

            maxPlayerHealth = maxPlayerHealth * 2;
            currentPlayerHealth = maxPlayerHealth;
            healthBar.SetMaxHealth(maxPlayerHealth);
            healthBar.SetPlayerHealth(currentPlayerHealth);

            Collider[] assustados = Physics.OverlapSphere(transform.position, 10 * masterSizeValue, (1<<7) );
            foreach (Collider scaredenemy in assustados)
            {
                scaredenemy.SendMessage("GetScared", SendMessageOptions.DontRequireReceiver);

            }


            do
            {
                transform.localScale = Vector3.Lerp(startScale, maxScale, timerForGrowth / growTime);
                

                vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = Mathf.Lerp(vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, (vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y + cameraOffsetAdjustToSize), timerForGrowth / growTime);
                vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = Mathf.Lerp(vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z, (vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z - cameraOffsetAdjustToSize), timerForGrowth / growTime);

                healthBarSize.sizeDelta = new Vector2 (Mathf.Lerp(initHealthBarSize, (initHealthBarSize + healthBarAdjust), timerForGrowth / growTime), healthBarSize.sizeDelta.y);


                timerForGrowth += Time.deltaTime;

                yield return null;

            }
            while (timerForGrowth < growTime && isMaxSize == false);

            initHealthBarSize += healthBarAdjust;
            isMaxSize = true;
            IncreaseAllValuesToNewSize();
            GetComponent<PlayerMovement>().controlsDisabled = false;

        }



    }


    private void IncreaseAllValuesToNewSize()
    {




        startSize = maxSize;
        maxSize = maxSize * 1.5f;
        GetComponent<PlayerMovement>().playerDamage = GetComponent<PlayerMovement>().playerDamage * 2;
        GetComponent<PlayerMovement>().isAttacking = false;
        sizeCombatRadius = sizeCombatRadius + 0.75f;

        var warriors = FindObjectsOfType<Warrior>() ;
        foreach (var warrior in warriors)
        {
            warrior.AdjustStoppingDistance();
        }

        isMaxSize = false;

            // mudar o som pra sons mais graves - colocar novos clipes como referência, despois de editar no audacity

        timerForGrowth = 0;


    }


    private void Update()
    {
        if (regenCooldown >-1)
        {
        regenCooldown -= Time.deltaTime;
        }

        PlayerRegeneration();

        //FOR TESTING---------------------------------------------------------------------------------
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerIncreaseGrowth(10);
        }

    }

    private void PlayerRegeneration()
    {

        if (regenCooldown <= 0 && (currentPlayerHealth < maxPlayerHealth) && currentPlayerHealth > 0)
        {
            currentPlayerHealth++;
            healthBar.SetPlayerHealth(currentPlayerHealth);
            regenCooldown = 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sizeCombatRadius);


    }



    private void OnParticleCollision(GameObject other) // being hit by arrows/magic 
    {
        if(other.layer == 7)
        {

            if (other.GetComponentInParent<CasterEnemy>() )
            {
                if(other.GetComponentInParent<CasterEnemy>().activateFireBallDamage == true)
                {
                    PlayerTakeDamage(other.GetComponentInParent<CasterEnemy>().fireballDMG);
                }
                else
                {
                    PlayerTakeDamage(other.GetComponentInParent<CasterEnemy>().enemyDamage);
                }



            }


            else
            {
                PlayerTakeDamage(other.GetComponentInParent<RangedEnemy>().enemyDamage);

            }

        }
    }


}
