using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainCounter : MonoBehaviour
{
    public GameObject T;
    public GameObject R;
    public GameObject B;
    public GameObject L ;


    public List<GameObject> terrainBlocks;

    // Start is called before the first frame update
    void Start()
    {
      //  StartCoroutine(SpawnBossBlock());
    }

    //IEnumerator SpawnBossBlock()
    //{

    //    float timeToSpawnBoss = 7;

    //    do
    //    {
    //        timeToSpawnBoss -= Time.deltaTime;
    //        yield return null;
    //    }
    //    while (timeToSpawnBoss <= 0);


    //    var lastTerrainBlock = terrainBlocks.LastOrDefault();

    //    Destroy(lastTerrainBlock);

        //if (terrainBlocks.Contains(B))
        //{
        //    Debug.Log("achou B");
        //}
        //if (terrainBlocks.Contains(T))
        //{
        //    Debug.Log("achou T");
        //}
        //if (terrainBlocks.Contains(R))
        //{
        //    Debug.Log("achou R");
        //}
        //if (terrainBlocks.Contains(L))
        //{
        //    Debug.Log("achou L");
        //}


  //  }


    // Update is called once per frame
    void Update()
    {
        
    }
}
