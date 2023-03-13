using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTCUBE : MonoBehaviour
{

    public ParticleSystem rangedAttackParticleSystem;
    public Transform rangedAttackTarget;
    public Transform rangedAttackStartingPoint;
    [SerializeField] float turnSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rangedAttackTarget = FindObjectOfType<RangedAttackTarget>().transform;
    }

    void AimWhenAttackingRanged()
    {
        Vector3 direction = (rangedAttackTarget.position - transform.position);

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Quaternion particleSystemRotation = rangedAttackParticleSystem.transform.rotation;
        particleSystemRotation.eulerAngles = direction;

        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, (direction.y + (target.GetComponent<PlayerHealth>().masterSizeValue - 1) * 0.1f), direction.z));


        // rangedAttackParticleSystem.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed*3);

        rangedAttackParticleSystem.transform.rotation = lookRotation;

    }

    void FaceTargetWhenAttacking()
    {
        Vector3 direction = (rangedAttackTarget.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        FaceTargetWhenAttacking();

        AimWhenAttackingRanged();
        if(!rangedAttackParticleSystem.isPlaying)
        {
            rangedAttackParticleSystem.Play();
        }
    }
}
