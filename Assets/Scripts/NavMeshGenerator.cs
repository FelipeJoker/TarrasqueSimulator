using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;


public class NavMeshGenerator : MonoBehaviour
{

    public Unity.AI.Navigation.NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GenerateNewMesh", 7);
    }
    
    public void GenerateNewMesh()
    {
        surface.RemoveData();
        surface.BuildNavMesh();
    }
    
}
