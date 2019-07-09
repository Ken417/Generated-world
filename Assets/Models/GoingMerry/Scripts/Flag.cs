﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    float _Speed;
    [SerializeField, Range(0, 10f)]
    float _Frequency;
    [SerializeField,Range(0, 10f)]
    float _Amplitude;

    bool update = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<MeshRenderer>().material.SetVector("_Axis",GameObject.Find("Axis1").transform.position);
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            int matNum = mr.sharedMaterials.Length;
            Material[] mat = new Material[matNum];
            for (int i = 0; i < matNum; i++)
            {
                mat[i] = new Material(Shader.Find("Custom/Flag"));
                mat[i].color = mr.materials[i].color;
            }
            mr.materials = mat;
        }
    }

    void Refresh()
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            int matNum = mr.sharedMaterials.Length;
            Material[] mat = mr.materials;
            for (int i = 0; i < matNum; i++)
            {
                mat[i].SetFloat("_Speed", _Speed);
                mat[i].SetFloat("_Frequency", _Frequency);
                mat[i].SetFloat("_Amplitude", _Amplitude);
            }
            mr.materials = mat;
        }
    }

    void OnValidate()
    {
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(update)
        {
            Refresh();
            update = false;
        }
    }
}
