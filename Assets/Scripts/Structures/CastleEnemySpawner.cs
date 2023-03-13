using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CastleEnemySpawner : MonoBehaviour
{


    private Queue warriorPool;
    [SerializeField] GameObject warriorPrefab;
    [SerializeField] int poolSize = 10;
    [SerializeField] float spawnTimer = 3;
    [SerializeField] Transform parent;

    GameObject[] pool;

    public Transform spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        PopulatePool();
        StartCoroutine(SpawnEnemy());
        
    }
    void PopulatePool()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < pool.Length; i++)
        {
            if (spawnPoint != null)
            {
                pool[i] = Instantiate(warriorPrefab, spawnPoint.position, Quaternion.Euler(0,180,0));
                pool[i].SetActive(false);
                pool[i].transform.parent = parent;
            }


        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (spawnPoint != null)
            {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);

            }
        }
    }

    private void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                if (spawnPoint != null)
                {
                    pool[i].GetComponent<NavMeshAgent>().Warp(spawnPoint.position);
                    pool[i].SetActive(true);
                    return;

                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
