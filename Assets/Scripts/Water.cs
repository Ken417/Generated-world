using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.mainTexture = TextureGenerator.lastUsedParlinNoiseTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
