using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawArea : MonoBehaviour
{

    public bool Clawing = false;

    public float pushForce = 100;

    public GameObject structDamageFX;
    public GameObject structDamageFX2;
    public GameObject EnemyHitFX;
    public GameObject EnemyHitFX2;
    public GameObject treeDamageFX;

    public AudioSource audioSource;
    public AudioClip impact1;
    public AudioClip impact2;




    private void OnTriggerEnter(Collider other)
    {
        if (Clawing)
        {
            

            if (other.gameObject.GetComponent<EnemyStats>() != null && other.gameObject.GetComponent<EnemyStats>().isDead == false)
            {

                other.gameObject.GetComponent<EnemyStats>().TakeDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX2, other.ClosestPoint(transform.position), Quaternion.identity);
                if(audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact1);
                }

            }


            if (other.gameObject.GetComponent<StructureHealth>() != null)
            {
                    other.gameObject.GetComponent<StructureHealth>().StructureTakeDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                    Instantiate(structDamageFX, other.ClosestPoint(transform.position), Quaternion.identity) ;
                    Instantiate(structDamageFX2, other.ClosestPoint(transform.position), Quaternion.identity);
                if (audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact2);
                }
            }


            if (other.gameObject.GetComponent<Trees>() != null)
            {
                other.gameObject.GetComponent<Trees>().TreeTakeDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                if (audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact2);
                }
            }

            if (other.gameObject.GetComponent<Destructibles>() != null)
            {
                other.gameObject.GetComponent<Destructibles>().PropTakesDamage(GetComponentInParent<PlayerMovement>().playerDamage);
                Instantiate(treeDamageFX, other.ClosestPoint(transform.position), Quaternion.identity);
                Instantiate(EnemyHitFX, other.ClosestPoint(transform.position), Quaternion.identity);
                if (audioSource.isPlaying == false)
                {
                    audioSource.PlayOneShot(impact2);
                }
            }

            

        }


    }



    private void OnCollisionEnter(Collision other)
    {



    }

    




}
