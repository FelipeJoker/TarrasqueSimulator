using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootArea : MonoBehaviour
{

    public bool SteppingR = false;
    public bool thisIsTheRightFoot = false;

    public bool SteppingL = false;
    public bool thisIsTheLeftFoot = false;

    public AudioSource audioSource;

    public ParticleSystem walkDirtParticles;
    public GameObject structDamageFX;
    public GameObject structDamageFX2;
    public GameObject EnemyHitFX;
    public GameObject EnemyHitFX2;
    public GameObject treeDamageFX;


    public AudioClip impact1;
    public AudioClip impact2;

    public void PlayStepSound()
    {
        audioSource.Play();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == default)
        {
            walkDirtParticles.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == default)
        {
            walkDirtParticles.Play();
        }





        if (SteppingR == true && thisIsTheRightFoot == true)

        {

            if (other.gameObject.GetComponent<EnemyStats>() != null && other.gameObject.GetComponent<EnemyStats>().isDead == false)
            {

                other.gameObject.GetComponent<EnemyStats>().TakeDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX2, other.ClosestPoint(transform.position), Quaternion.identity);
                if (audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact1);
                }

            }
           

            if (other.gameObject.GetComponent<Destructibles>() != null)
            {
                other.gameObject.GetComponent<Destructibles>().PropTakesDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                audioSource.PlayOneShot(impact2);

            }


            if (other.gameObject.GetComponent<Trees>() != null)
            {
                other.gameObject.GetComponent<Trees>().TreeTakeDamage(100);// Hard coded number to always destroy trees
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                audioSource.PlayOneShot(impact2);
            }

        }


        if (SteppingL == true && thisIsTheLeftFoot == true)

        {

            if (other.gameObject.GetComponent<EnemyStats>() != null && other.gameObject.GetComponent<EnemyStats>().isDead == false)
            {

                other.gameObject.GetComponent<EnemyStats>().TakeDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX2, other.ClosestPoint(transform.position), Quaternion.identity);
                if (audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact1);
                }

            }
        

            if (other.gameObject.GetComponent<Destructibles>() != null)
            {
                other.gameObject.GetComponent<Destructibles>().PropTakesDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                audioSource.PlayOneShot(impact2);

            }

            if (other.gameObject.GetComponent<Trees>() != null)
            {
                other.gameObject.GetComponent<Trees>().TreeTakeDamage(100);// Hard coded number to always destroy trees
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                audioSource.PlayOneShot(impact2);

            }


        }


    }

    




}

