using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.SetVector("_Axis",GameObject.Find("Axis1").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
