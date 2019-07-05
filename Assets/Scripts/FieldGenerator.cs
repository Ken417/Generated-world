﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]//自動でコンポーネントをアタッチしてくれる
public class FieldGenerator : MonoBehaviour
{
    const int defaultResolution = 100;//基本resolution
    [Header("一辺の四角の枚数"), Range(1, 200), SerializeField]
    int resolution = defaultResolution;
    int currentResolution;

    Vector3 offset;
    Vector3 rotation;

    [Header("パーリンノイズを使うかどうか"), SerializeField]
    bool usePerlinNoise = true;

    [Header("パーリンノイズのシード"), SerializeField]
    Vector2 perlinNoiseSeed;


    [Header("周波数"), SerializeField]
    float frequency = 3f;

    [Header("オクターブ(違う周波数のパーリンノイズをいくつ組み合わせるか)"), Range(1, 8), SerializeField]
    int octaves = 5;

    [Header("オクターブで周波数をどれだけ変えるか"), Range(1f, 4f), SerializeField]
    float lacunarity = 2f;

    [Header("オクターブをどれだけ影響させるか"), Range(0f, 1f), SerializeField]
    float persistence = 0.5f;

    [Header("最大高度"), Range(0f, 1f), SerializeField]
    float maxHeight = 1;

    [Header("最小高度"), Range(-0.5f,0), SerializeField]
    float minHeight = -0.2f;

    [Header("最低高度にどれだけ元の高度をのせるか"), Range(0f, 1f), SerializeField]
    float minHeightRewind = 0.15f;


    [Header("木を生やすかどうか"), SerializeField]
    bool useTree = false;

    [Header("マスクを使うかどうか"), SerializeField]
    bool useMask = true;


    [Header("パーリンノイズマスクのシード"), SerializeField]
    Vector2 pNoiseMaskSeed;

    [Header("パーリンノイズのマスクの周波数"), SerializeField]
    float pNoiseMaskFrequency = 3;

    [Header("パーリンノイズマスクの影響レベル"), Range(0f, 2f), SerializeField]
    float pNoiseMaskAmount = 0;

    [Header("サークルマスクの半径"), Range(0f, 100f), SerializeField]
    float cercleMaskRadius = 0;

    [Header("サークルマスクマスクの影響レベル"), Range(-0.5f, 0.5f), SerializeField]
    float cercleMaskAmount = 0;

    [Header("エッジマスクの幅"), Range(0f, 100f), SerializeField]
    float edgeMaskWidth = 0;

    [Header("エッジマスクマスクの影響レベル"), Range(-0.5f, 0.5f), SerializeField]
    float edgeMaskAmount = 0;


    [Header("山の色"), SerializeField]
    Gradient coloring;

    [Header("砂の色"), SerializeField]
    Color sandColor;

    float minVertices = 1;
    float maxVertices = 0;


    Mesh _mesh;
    public Mesh mesh
    {
        get
        {
            return _mesh;
        }
    }

    Vector3[] vertices;
    Vector3[] verticesMask;
    Vector3[] normals;
    Color[] colors;

    bool update = true;

    void OnEnable()//コンポーネントがアクティブになったときに呼ばれる
    {
        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = _mesh;
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Surface Shader"));
            //GetComponent<MeshRenderer>().material.mainTexture = TextureGenerator.CreateParlinNoseTexture(resolution/2, resolution/2, perlinNoiseSeed, 30);
            //GetComponent<MeshRenderer>().material.mainTexture = TextureGenerator.CreateVoronoiDiagramTexture(100,100);
            GetComponent<MeshRenderer>().material.mainTexture = TextureGenerator.CreateRandomNoiseTexture(100,100);
            GetComponent<MeshRenderer>().material.SetTexture("_ValueNoiseTex",TextureGenerator.CreateRandomNoiseTexture(100,100));
            //GetComponent<MeshRenderer>().material.SetTexture("_ParlinNoiseTex", TextureGenerator.CreateParlinNoiseTexture(100,100, perlinNoiseSeed, 30));
        }
        Refresh();
    }

    void Start()
    {
        GameObject.Find("TEST").GetComponent<MeshRenderer>().material.mainTexture = TextureGenerator.CreateVoronoiDiagramTexture(200, 200);

        //これやらないとなぜか当たらない
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<MeshCollider>().enabled = true;
        }
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

    void OnValidate()
    {
        update = true;
    }
    void Refresh()
    {
        if (resolution != currentResolution)
        {
            CreateGrid();
        }
        if(useMask)
        {
            AddMask();
        }
        if (usePerlinNoise)
        {
            AddPerlinNoise();
        }
        SetRegion();
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = _mesh;
        
        if(useTree)
        {
            GameObject.Find("TreeManager").GetComponent<TreeManager>().SetTree();
        }
    }

    void AddPerlinNoise()
    {
        for (int v = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, v++)
            {
                float amplitude = 1f;
                float range = 1f;
                float sample = 0;
                float freq = frequency;
                if (useMask)
                {
                    sample = verticesMask[v].y;
                }
                for (int o = 0; o< octaves;++o)
                {
                    float xSample = ((float)x/ resolution + perlinNoiseSeed.x) * freq;
                    float zSample = ((float)z/ resolution + perlinNoiseSeed.y) * freq;
                    sample += (Mathf.PerlinNoise(xSample, zSample)-0.5f)* amplitude * maxHeight;

                    freq *= lacunarity;
                    amplitude *= persistence;
                    range += amplitude;
                }
                sample /= range;

                //高度の低い場所の高さを上げる
                if (sample < minHeight)
                {
                    sample = minHeight - (minHeight-sample) * minHeightRewind;
                }

                vertices[v].y = sample;
                colors[v] = coloring.Evaluate(sample/ maxHeight + 0.5f);
            }
        }

        _mesh.vertices = vertices;
    }


    void SetRegion()
    {
        for (int z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                int v = resolution;
                float py = vertices[z * v + x].y;

                //岩
                if (-0.01f>py)
                {
                    colors[z * v + x] = new Color(0, 1, 0, 0);
                    continue;
                }

                //丘
                if(py < 0)
                {
                    colors[z * v + x] = new Color(0, 1, 0, 0);
                }
                else
                {
                    colors[z * v + x] = new Color(1, 0, 0, 0);
                }


                if (py > 0.01f) {continue;}

                Vector3 p1 = vertices[z * v + x];
                Vector3 p2;
                p2 = vertices[(z+1> v?z:z+ 1) * v + x];
                float angle = Vector3.Angle(p1, p2);

                p2 = vertices[z * v + (x + 1 > v ? x : x +1)];
                angle += Vector3.Angle(p1, p2);

                p2 = vertices[(z - 1 < 0 ? z : z - 1) * v + x];
                angle += Vector3.Angle(p1, p2);

                p2 = vertices[z * v + (x - 1 < 0 ? x :x - 1)];
                angle += Vector3.Angle(p1, p2);


                p2 = vertices[(z + 1 > v ? z : z + 1) * v + (x - 1 < 0 ? x : x - 1)];
                angle += Vector3.Angle(p1, p2);

                p2 = vertices[(z + 1 > v ? z : z + 1) * v + (x + 1 > v ? x : x + 1)];
                angle += Vector3.Angle(p1, p2);

                p2 = vertices[(z - 1 < 0 ? z : z - 1) * v + (x - 1 < 0 ? x : x - 1)];
                angle += Vector3.Angle(p1, p2);

                p2 = vertices[(z - 1 < 0 ? z : z - 1) * v + (x + 1 > v ? x : x + 1)];
                angle += Vector3.Angle(p1, p2);

                //浜
                if (angle/8 < 2)
                {
                    colors[z * v + x] = Color.black;
                }

                //岩
                if (angle / 8 > 5)
                {
                    colors[z * v + x] = new Color(0, 1, 0, 0);
                }
            }
        }
    }
    void AddMask()
    {
        verticesMask = vertices;
        int p = resolution / 2;
        float r = cercleMaskRadius * ((float)resolution / defaultResolution);
        r *= r;
        for (int v = 0,z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, v++)
            {
                verticesMask[v].y = 0f;

                //サークルマスク
                if ((x - p) * (x - p) + (z - p) * (z - p) < r)
                {
                    verticesMask[v].y = (r - ((x - p) * (x - p) + (z - p) * (z - p))) * (cercleMaskAmount / r);
                }

                float xSample = ((float)x / resolution + pNoiseMaskSeed.x) * pNoiseMaskFrequency;
                float zSample = ((float)z / resolution + pNoiseMaskSeed.y) * pNoiseMaskFrequency;
                verticesMask[v].y += (Mathf.PerlinNoise(xSample, zSample)-0.5f) * pNoiseMaskAmount;

                //エッジマスク
                float emWidth = edgeMaskWidth * ((float)resolution / defaultResolution);
                if (emWidth > z|| (resolution - emWidth) <z || emWidth > x || (resolution - emWidth) < x)
                {
                    float minZ = z;
                    if ((resolution - z) < z) { minZ = (resolution - z); }
                    float minX = x;
                    if ((resolution - x) < x) { minX = (resolution - x); }


                    if (minZ < minX)
                    {
                        if ((resolution - emWidth) < z)
                        {
                            verticesMask[v].y += (emWidth - (resolution - z)) * edgeMaskAmount;
                        }
                        else if (emWidth > z)
                        {
                            verticesMask[v].y += (emWidth - z) * edgeMaskAmount;
                        }
                    }
                    else
                    {
                        if ((resolution - emWidth) < x)
                        {
                            verticesMask[v].y += (emWidth - (resolution - x)) * edgeMaskAmount;
                        }
                        else if (emWidth > x)
                        {
                            verticesMask[v].y += (emWidth - x) * edgeMaskAmount;
                        }
                    }
                }

                //colors[v] = coloring.Evaluate(verticesMask[v].y * colorWidth+(colorPos/10));
            }
        }
        _mesh.vertices = verticesMask;
    }


    void CreateGrid()
    {
        currentResolution = resolution;
        _mesh.Clear();
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        colors = new Color[vertices.Length];
        normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        float stepSize = 1f / resolution;
        for (int v = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
                colors[v] = Color.black;
                normals[v] = Vector3.up;
                uv[v] = new Vector2(x * stepSize, z * stepSize);
            }
        }
        _mesh.vertices = vertices;
        _mesh.colors = colors;
        _mesh.normals = normals;
        _mesh.uv = uv;

        int[] triangles = new int[resolution * resolution * 6];
        for (int t = 0, v = 0, y = 0; y < resolution; y++, v++)
        {
            for (int x = 0; x < resolution; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + resolution + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + resolution + 1;
                triangles[t + 5] = v + resolution + 2;
            }
        }
        _mesh.triangles = triangles;
    }

}

[CustomEditor(typeof(FieldGenerator))]//拡張するクラスを指定
public class ExampleScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        //targetを変換して対象を取得
        FieldGenerator fg = target as FieldGenerator;

        //ボタンを表示
        if (GUILayout.Button("メッシュをアセットとして保存") && fg.mesh)
        {
            AssetDatabase.CreateAsset(fg.mesh, "Assets/Field.asset");
            AssetDatabase.SaveAssets();
        }
    }

}