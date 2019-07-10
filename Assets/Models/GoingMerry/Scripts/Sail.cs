using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)]
    float _Wind;

    bool update = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            int matNum = mr.sharedMaterials.Length;
            Material[] mat = new Material[matNum];
            for (int i = 0; i < matNum; i++)
            {
                mat[i] = new Material(Shader.Find("Custom/Sail"));
                mat[i].color = mr.materials[i].color;
            }
            mr.materials = mat;
            MeshFilter mf = mr.GetComponent<MeshFilter>();
            mf.mesh.bounds = new Bounds(mf.mesh.bounds.center, mf.mesh.bounds.size*3);
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
                mat[i].SetFloat("_Wind", _Wind);
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
        _Wind = (Mathf.Sin(Time.time/2)+1)/2;
        update = true;
        if (update)
        {
            Refresh();
            update = false;
        }
    }
}
