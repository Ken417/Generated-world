using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMeshCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(MeshFilter mf in GetComponentsInChildren<MeshFilter>())
        {
            MeshCollider mc = mf.gameObject.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.sharedMesh;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
