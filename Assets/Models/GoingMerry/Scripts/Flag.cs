using System.Collections;
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
        //コンバインできない
        //MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
        //CombineInstance[] combine = new CombineInstance[mfs.Length];
        //for(int i =0;i< mfs.Length;i++)
        //{
        //    combine[i].mesh = mfs[i].sharedMesh;
        //    combine[i].transform = mfs[i].transform.localToWorldMatrix;
        //    mfs[i].gameObject.SetActive(false);
        //}

        //MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        //meshFilter.mesh = new Mesh();
        //meshFilter.mesh.CombineMeshes(combine);

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
