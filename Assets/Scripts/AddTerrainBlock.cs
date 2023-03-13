using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTerrainBlock : MonoBehaviour
{

    private TerrainCounter terrainBlockscounter;





    // Start is called before the first frame update
    void Start()
    {
        terrainBlockscounter = GameObject.FindObjectOfType<TerrainCounter>();
        terrainBlockscounter.terrainBlocks.Add(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
