using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureHealth : MonoBehaviour
{

    public int maxStructureHealth = 5;
    public int currentStructureHealth;

    public int timerForDestruction = 9;

    public GameObject destroyedVersion;

    public Color startColor;
    public GameObject particleSystemAttack;

    public float timeToDestroyDestroyedVersion = 10;

    // Start is called before the first frame update
    void Start()
    {
        currentStructureHealth = maxStructureHealth;
       
    }

    public void StructureTakeDamage(int damage)
    {
        currentStructureHealth -= damage;

            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshes)
            {
            startColor = mesh.material.color;
            mesh.material.color = new Color((startColor.r - 0.05f), (startColor.g - 0.05f), (startColor.b - 0.05f));
            }

        if(currentStructureHealth <=0)
        {
            if(particleSystemAttack !=null)
            {
            Destroy(particleSystemAttack);
            }

            StartCoroutine (UnparentAndDestroy());
            GameObject destroyed = (GameObject)  Instantiate(destroyedVersion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(destroyed, timeToDestroyDestroyedVersion);


        }

    }

    IEnumerator UnparentAndDestroy()
    {

            foreach (Transform child in transform)
            {
                if (child.GetComponent<Warrior>() != null)
                {
                    child.transform.parent = null;
                }
            }
            yield return null;
       
    }


    // Update is called once per frame
    void Update()
    {
        

    }
}
