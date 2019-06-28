using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]//自動でコンポーネントをアタッチしてくれる
public class FieldGenerator : MonoBehaviour
{
    [Header("一辺の四角の枚数"), Range(1, 200), SerializeField]
    int resolution = 100;
    int currentResolution;

    Vector3 offset;
    Vector3 rotation;

    [Header("周波数"), SerializeField]
    float frequency = 3f;

    [Header("オクターブ(違う周波数のパーリンノイズをいくつ組み合わせるか)"), Range(1, 8), SerializeField]
    int octaves = 5;

    [Header("オクターブで周波数をどれだけ変えるか"), Range(1f, 4f), SerializeField]
    float lacunarity = 2f;

    [Header("オクターブをどれだけ影響させるか"), Range(0f, 1f), SerializeField]
    float persistence = 0.5f;

    [Header("山の色"), SerializeField]
    Gradient coloring;

    [Header("山の色の幅"), SerializeField]
    float colorWidth = 1;

    [Header("山の色位置"), SerializeField]
    float colorPos = 0;


    Mesh _mesh;
    public Mesh mesh
    {
        get
        {
            return _mesh;
        }
    }

    Vector3[] vertices;
    Vector3[] normals;
    Color[] colors;

    Vector2 seed;

    void OnEnable()//コンポーネントがアクティブになったときに呼ばれる
    {
        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = _mesh;
        }
        seed.x = Random.value;
        seed.y = Random.value;
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Surface Shader"));
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        if (resolution != currentResolution)
        {
            CreateGrid();
        }
        AddPerlinNoise();
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
                for (int o = 0; o< octaves;++o)
                {
                    float xSample = ((float)x/ resolution + seed.x ) * freq;
                    float zSample = ((float)z/ resolution + seed.y ) * freq;
                    sample += Mathf.PerlinNoise(xSample, zSample)* amplitude;
                    freq *= lacunarity;
                    amplitude *= persistence;
                    range += amplitude;
                }
                sample /= range;
                vertices[v].y = sample;
                colors[v] = coloring.Evaluate(sample*colorWidth+(colorPos/10));
            }
        }

        _mesh.vertices = vertices;
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
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
