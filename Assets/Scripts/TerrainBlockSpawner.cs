using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBlockSpawner : MonoBehaviour
{

    public int OpeningDirection;
    //1 - need top door
    //2 - Need right door
    //3 - need bottom door
    //4 - need left door


    public TerrainBlocksTemplates templates;
    private int rand;
    public bool spawned = false;

    private Vector3 terrainOffset = new Vector3 (0f, 10f, 0f);

 

    public string thisSpawnerTemplates;

    public TerrainCounter terrainBlockscounter;

    public GameObject TR;
    public GameObject TB;
    public GameObject TL;
    public GameObject RB;
    public GameObject RL;
    public GameObject BL;

    public GameObject T;
    public GameObject R;
    public GameObject B;
    public GameObject L;

    public bool isEndT;
    public bool isEndR;
    public bool isEndB;
    public bool isEndL;

    public bool doNotGoTop;
    public bool doNotGoRight;
    public bool doNotGoBottom;
    public bool doNotGoLeft;


    public float timeToSpawn = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
       


        terrainBlockscounter = GameObject.FindObjectOfType<TerrainCounter>();

        if(spawned == false)
        {
            Invoke ("SpawnTerrainBlocks", timeToSpawn);
        }
    }


    void SpawnTerrainBlocks()
    {
        if (spawned == false)
        {
            

            templates = GameObject.FindGameObjectWithTag(thisSpawnerTemplates).GetComponent<TerrainBlocksTemplates>();

            if (OpeningDirection == 1)
            {
                rand = Random.Range(0, templates.topTerrainBlocks.Length);
                GameObject InstantiatedBlock = (GameObject) Instantiate(templates.topTerrainBlocks[rand], transform.position + new Vector3(0,-100,0), Quaternion.identity);

                if(terrainBlockscounter.terrainBlocks.Count <3)
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                     childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "MiddleTemplates";

                    }
                }
                else
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "EndTemplates";

                    }
                }
                
            }
            else if (OpeningDirection == 2)
            {
                rand = Random.Range(0, templates.rightTerrainBlocks.Length);
                GameObject InstantiatedBlock = Instantiate(templates.rightTerrainBlocks[rand], transform.position + new Vector3(0, -100, 0), Quaternion.identity);

                if (terrainBlockscounter.terrainBlocks.Count < 3)
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "MiddleTemplates";

                    }
                }
                else
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "EndTemplates";

                    }
                }

            }
            else if (OpeningDirection == 3)
            {
                rand = Random.Range(0, templates.bottomTerrainBlocks.Length);
                GameObject InstantiatedBlock = Instantiate(templates.bottomTerrainBlocks[rand], transform.position + new Vector3(0, -100, 0), Quaternion.identity);

                if (terrainBlockscounter.terrainBlocks.Count < 3)
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "MiddleTemplates";

                    }
                }
                else
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "EndTemplates";

                    }
                }

            }
            else if (OpeningDirection == 4)
            {
                rand = Random.Range(0, templates.leftTerrainBlocks.Length);
                GameObject InstantiatedBlock = Instantiate(templates.leftTerrainBlocks[rand], transform.position + new Vector3(0, -100, 0), Quaternion.identity);

                if (terrainBlockscounter.terrainBlocks.Count < 3)
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "MiddleTemplates";

                    }
                }
                else
                {
                    foreach (var childSpawner in InstantiatedBlock.GetComponentsInChildren<TerrainBlockSpawner>())
                    {
                        childSpawner.GetComponent<TerrainBlockSpawner>().thisSpawnerTemplates = "EndTemplates";

                    }
                }

            }

            spawned = true;
           
        }
    }





    private void OnTriggerEnter(Collider other)
    {



        if (other.CompareTag("SpawnPoint") && other.GetComponent<TerrainBlockSpawner>().spawned == false && spawned == false)
        {

            Debug.Log(gameObject.name + "de " + transform.parent.name +  " collided! with " + other.name + "de " +other.transform.parent + transform.position);


            if (this.OpeningDirection ==1 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 2)
            {
                GameObject _TR_ = (GameObject)Instantiate(TR, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TR_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 2 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 1)
            {
                GameObject _TR = (GameObject)Instantiate(TR, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TR.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }


            if (this.OpeningDirection == 1 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 3)
            {
                GameObject _TB_ = (GameObject)Instantiate(TB, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TB_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 3 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 1)
            {
                GameObject _TB = (GameObject)Instantiate(TB, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TB.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 1 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 4)
            {
                GameObject _TL_ = (GameObject)Instantiate(TL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TL_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 4 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 1)
            {
                GameObject _TL = (GameObject)Instantiate(TL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _TL.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 2 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 3)
            {
                GameObject _RB_ = (GameObject)Instantiate(RB, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _RB_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 3 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 2)
            {
                GameObject _RB = (GameObject) Instantiate(RB, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _RB.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }


            if (this.OpeningDirection == 2 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 4)
            {
                GameObject _RL_ = (GameObject)Instantiate(RL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _RL_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 4 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 2)
            {
                GameObject _RL = (GameObject) Instantiate(RL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _RL.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 3 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 4)
            {
                GameObject _BL_ = (GameObject)Instantiate(BL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _BL_.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            if (this.OpeningDirection == 4 && other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 3)
            {
                GameObject _BL = (GameObject) Instantiate(BL, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                foreach (var child in _BL.GetComponentsInChildren<TerrainBlockSpawner>())
                {
                    child.GetComponent<TerrainBlockSpawner>().spawned = true;
                }
            }

            spawned = true;


        }

        //if (other.CompareTag("SpawnPoint") && other.GetComponent<TerrainBlockSpawner>().spawned == false && spawned == true) // se eu já tiver criado e o outro não...
        //{
        //    other.GetComponent<TerrainBlockSpawner>().spawned = true;
        //}



        if (other.CompareTag("SpawnPoint") && other.GetComponent<TerrainBlockSpawner>().spawned == true && spawned == false)
        {
                if(other.GetComponent<TerrainBlockSpawner>().OpeningDirection ==0 && other.GetComponent<TerrainBlockSpawner>().isEndL == false && other.GetComponent<TerrainBlockSpawner>().isEndT == false && other.GetComponent<TerrainBlockSpawner>().isEndB == false && other.GetComponent<TerrainBlockSpawner>().isEndR == false) // se bater num outro spawner, que já criou, e for o do meio...
                {
                
                spawned = true;

                }

                if (other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 0 && other.GetComponent<TerrainBlockSpawner>().isEndL == true)
                {
                   
                    Instantiate(T, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                    spawned = true;
                }

                if (other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 0 && other.GetComponent<TerrainBlockSpawner>().isEndT == true)
                {
                   
                    Instantiate(L, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                    spawned = true;
                }

                if (other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 0 && other.GetComponent<TerrainBlockSpawner>().isEndR == true)
                {
                    
                    Instantiate(B, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                    spawned = true;
                }

                if (other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 0 && other.GetComponent<TerrainBlockSpawner>().isEndB == true)
                {
                   
                    Instantiate(R, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                    spawned = true;
                }

                if (other.GetComponent<TerrainBlockSpawner>().OpeningDirection == 0 && OpeningDirection == 0) // se forem 2 meios, e o outro já tiver dado spawn
                {
                    Debug.Log("bateram meios em " + transform.position);
                    spawned = true;
                }



                if (other.GetComponent<TerrainBlockSpawner>().doNotGoTop)
                {
                    if(OpeningDirection == 4)
                    {
                        Instantiate(L, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 3)
                    {
                        Instantiate(B, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 2)
                    {
                        Instantiate(R, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }


                }

                if (other.GetComponent<TerrainBlockSpawner>().doNotGoRight)
                {

                    if (OpeningDirection == 3)
                    {
                        Instantiate(B, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 4)
                    {
                        Instantiate(L, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 1)
                    {
                        Instantiate(T, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                }

                if (other.GetComponent<TerrainBlockSpawner>().doNotGoBottom)
                {

                    if (OpeningDirection == 4)
                    {
                        Instantiate(L, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 1)
                    {
                        Instantiate(T, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 2)
                    {
                        Instantiate(R, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }


                }

                if (other.GetComponent<TerrainBlockSpawner>().doNotGoLeft)
                {

                    if (OpeningDirection == 3)
                    {
                        Instantiate(B, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 1)
                    {
                        Instantiate(T, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                    if (OpeningDirection == 2)
                    {
                        Instantiate(R, transform.position + new Vector3(0, -100, 0), Quaternion.identity);
                        spawned = true;
                    }

                }



        }







    }

    

}
