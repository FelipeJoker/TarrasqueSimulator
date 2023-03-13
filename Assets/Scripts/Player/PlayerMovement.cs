using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool controlsDisabled = false;

    public CharacterController controller;

    public Rigidbody rb;

    public Joystick joystick;

    public float speed = 6f;
    public float horizontal;
    public float vertical;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float attacksCooldownStart = 0.8f;
    public float attacksCooldown = 0.7f;

    public LayerMask enemyLayer;
    public LayerMask structuresLayer;

    public int playerDamage = 1;

    public Animator animator;

    private int clawComboCount = 0;
    public bool isAttacking = false;

    public float gravity = -9.8f; 
    Vector3 velocityToFall;

    public AudioSource audioSource;
    public AudioClip claw1;
    public AudioClip claw2;
    public AudioClip claw3;
    public AudioClip bite;

    public GameObject clawR;
    public GameObject clawL;
    public GameObject biteArea;
    public GameObject footR;
    public GameObject footL;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + new Vector3(horizontal, 0, vertical) * speed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(attacksCooldown >0)
        {
            attacksCooldown -= Time.deltaTime;
        }


        if (controlsDisabled == false)
        {
            horizontal =  joystick.Horizontal;
            vertical = joystick.Vertical;
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0);
                animator.SetTrigger("walk");
            }
            else
            {
                animator.SetTrigger("idle");
            }



        } // if controls disabled end

    } //update end

    public void NewBite()
    {
        if (attacksCooldown <= 0)
        {
            attacksCooldown = attacksCooldownStart;
            BiteStart();
        }
    }

    public void NewClaw()
    {
        if (attacksCooldown <= 0)
        {
            attacksCooldown = attacksCooldownStart;

            if (clawComboCount <= 2)
            {
                clawComboCount++;
            }
            else
            {
                clawComboCount = 1;
            }


            AttackStart();
        }
    }




    private void BiteStart()
    {
            GetComponentInChildren<BiteArea>().Biting = true;
            biteArea.GetComponent<BoxCollider>().enabled = true;

            animator.SetTrigger("bite");
            GetComponent<AudioSource>().volume = 1;
            GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.05f);
            audioSource.PlayOneShot(bite);
       
    }

    public void BiteEnd() // chamado pela animãção
    {
        biteArea.GetComponent<BoxCollider>().enabled = false;
        GetComponentInChildren<BiteArea>().Biting = false;
    }


    private void AttackStart()
    {

        if(clawComboCount == 1)
        {
            animator.SetTrigger("claw1");



        }
        else if (clawComboCount == 2)
        {
            animator.SetTrigger("claw2");
 


        }
        else if (clawComboCount == 3)
        {
            animator.SetTrigger("claw3");



        }

    }


    public void ClawStartR()
    {
            
            clawR.GetComponent<BoxCollider>().enabled = true;
            GetComponentInChildren<ClawArea>().Clawing = true;
    }

    public void ClawStartL()
    {
           clawL.GetComponent<BoxCollider>().enabled = true;
           GetComponentInChildren<ClawArea>().Clawing = true;
    }

    public void ClawStart()
    {
            clawL.GetComponent<BoxCollider>().enabled = true;
            clawR.GetComponent<BoxCollider>().enabled = true;
            GetComponentInChildren<ClawArea>().Clawing = true;
    }




    public void ClawSound1()
    {
        GetComponent<AudioSource>().volume = 0.7f;
        GetComponent<AudioSource>().pitch = 1;
        audioSource.PlayOneShot(claw1);
    }

    public void ClawSound2()
    {
        GetComponent<AudioSource>().volume = 0.7f;
        GetComponent<AudioSource>().pitch = 1;
        audioSource.PlayOneShot(claw2);
    }

    public void ClawSound3()
    {
        GetComponent<AudioSource>().volume = 0.7f;
        GetComponent<AudioSource>().pitch = 1;
        audioSource.PlayOneShot(claw3);
    }

    public void AttackEnd() // called from animation
    {

        clawL.GetComponent<BoxCollider>().enabled = false;
        clawR.GetComponent<BoxCollider>().enabled = false;



    }


    public void FootsCollidersOn() //lvl 3 consegue chutar os bichinhos
    {
        if (GetComponent<PlayerHealth>().masterSizeValue >=3)
        {
            footL.GetComponent<BoxCollider>().enabled = true;
            footR.GetComponent<BoxCollider>().enabled = true;

        }

    }

    public void HeavyStepRStart() // chamado pela anim
    {
        footR.GetComponent<FootArea>().SteppingR = true;
    }

    public void HeavyStepLStart() // chamado pela anim
    {
        footL.GetComponent<FootArea>().SteppingL = true;
    }

    public void HeavyStepSoundR() // chamado pela anim
    {
        if (GetComponent<PlayerHealth>().masterSizeValue >= 3)
        {
            footR.GetComponent<FootArea>().PlayStepSound();
        }
    }

    public void HeavyStepSoundL() // chamado pela anim
    {
        if (GetComponent<PlayerHealth>().masterSizeValue >= 3)
        {
            footL.GetComponent<FootArea>().PlayStepSound();

        }
    }


    public void HeavyStepLEnd() // chamado pela anim
    {
        footL.GetComponent<FootArea>().SteppingL = false;

    }

    public void HeavyStepREnd() // chamado pela anim
    {
        footR.GetComponent<FootArea>().SteppingR = false;

    }

}
