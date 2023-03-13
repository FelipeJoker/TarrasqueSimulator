using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamageRedirector : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {

        if (other.GetComponentInParent<Warrior>().activateFireBallDamage == true)
        {
            GetComponentInParent<PlayerHealth>().PlayerTakeDamage(other.GetComponentInParent<Warrior>().fireballDMG);
        }
        else
        {
            GetComponentInParent<PlayerHealth>().PlayerTakeDamage(other.GetComponentInParent<Warrior>().enemyDamage);

        }

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
