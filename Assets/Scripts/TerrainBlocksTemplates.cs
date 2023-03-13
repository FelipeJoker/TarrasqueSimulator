using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBlocksTemplates : MonoBehaviour
{
    
    public GameObject[] topTerrainBlocks;
    public GameObject[] rightTerrainBlocks;
    public GameObject[] bottomTerrainBlocks;
    public GameObject[] leftTerrainBlocks;

    public GameObject blockedTerrainBlock;

    public List<GameObject> terrainBlocks;



    // daqui pra baixo provavelmente poderia deletar.

    //public float waitTime;
    //private bool spawnedEnd;
    //public GameObject boss;

    //private Vector3 bossOffset = new Vector3(0, 2, 0);


    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (waitTime <= 0 && spawnedEnd == false)
    //    {    
    //      //  Instantiate(boss, terrainBlocks[terrainBlocks.Count-1].transform.position + bossOffset, Quaternion.identity);              ACTIVATE AFTER USING TEST AREA
    //        spawnedEnd = true;
    //    }
    //    else
    //    {
    //        if(waitTime > -10)
    //        {
    //        waitTime -= Time.deltaTime;
    //        }
    //    }

    //}
}
