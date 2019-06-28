using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateField : MonoBehaviour
{
    [Header("アセットとして書き出す"),SerializeField]
    bool createAssets = false;

    [Header("生成時に使うマテリアル"),SerializeField]
    Material material = null;

    [Header("フラットシェーディング風メッシュにするか"), SerializeField]
    bool flatMesh = false;

    [Header("フィールドの大きさ(セルの枚数)"),SerializeField]
    Vector2 fieldSize = new Vector2(10, 10);
    Vector2 verticesNum;//頂点の数

    [Header("x=起伏のなめらかさ,y=高さの最大値"), SerializeField]
    Vector2[] perlinNoise;

    // Start is called before the first frame update
    void Start()
    {
        verticesNum = new Vector2(fieldSize.x + 1, fieldSize.y + 1);


        GameObject field = CreatePlaneField();

        for (int i = 0;i <  perlinNoise.Length; ++i)
        {
            field = AddPerlinNoise(field, perlinNoise[i].x, perlinNoise[i].y);
        }

        if (createAssets)
        {
            AssetDatabase.CreateAsset(field.GetComponent<MeshFilter>().sharedMesh, "Assets/Field.asset");
            AssetDatabase.SaveAssets();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject CreatePlaneField()
    {
        if (!material)
        {
            material = new Material(Shader.Find("Standard"));
        }

        GameObject planeField = new GameObject("Field");
        MeshFilter filter = planeField.AddComponent<MeshFilter>();
        MeshRenderer renderer = planeField.AddComponent<MeshRenderer>();
        MeshCollider collider = planeField.AddComponent<MeshCollider>();

        //頂点を入れる変数
        Vector3[] vertices = new Vector3[(int)(verticesNum.x * verticesNum.y)];


        //頂点を並べる
        for (int x = 0; x < verticesNum.x; ++x)
        {
            for (int z = 0; z < verticesNum.y; ++z)
            {
                vertices[(int)(x + z * verticesNum.x)] = new Vector3(x - (verticesNum.x / 2), 0, z - (verticesNum.y / 2));
            }
        }

        //三角形の数
        int triangleNum = (int)((verticesNum.x) * (fieldSize.y)) * 2;

        //三角形を書く順番に頂点を入れる変数
        int[] triangles = new int[triangleNum * 3];

        //頂点を三角形になるよう並べる
        for (int x = 0; x < fieldSize.x; ++x)
        {
            for (int y = 0; y < fieldSize.y; ++y)
            {
                int squarePos = (int)(x + y * fieldSize.x);
                triangles[squarePos * 6 + 0] = (int)((x) + ((y + 1) * verticesNum.x));
                triangles[squarePos * 6 + 1] = (int)((x + 1) + (y * verticesNum.x));
                triangles[squarePos * 6 + 2] = (int)((x) + (y * verticesNum.x));
                triangles[squarePos * 6 + 3] = (int)((x + 1) + ((y + 1) * verticesNum.x));
                triangles[squarePos * 6 + 4] = (int)(triangles[squarePos * 6 + 1]);
                triangles[squarePos * 6 + 5] = (int)(triangles[squarePos * 6 + 0]);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //法線計算
        mesh.RecalculateNormals();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.material = material;

        return planeField;
    }

    GameObject AddPerlinNoise(GameObject field,float relief,float maxHeight)
    {
        MeshFilter mf = field.GetComponent<MeshFilter>();
        MeshCollider collider = field.GetComponent<MeshCollider>();

        Vector3[] vertices = new Vector3[(int)(verticesNum.x * verticesNum.y)];

        float seedX = Random.value * 100f;
        float seedZ = Random.value * 100f;
        for (int x = 0; x < verticesNum.x; ++x)
        {
            for (int z = 0; z < verticesNum.y; ++z)
            {
                float xSample = (x + seedX) / relief;
                float zSample = (z + seedZ) / relief;
                vertices[(int)(x + z * verticesNum.x)] = mf.sharedMesh.vertices[(int)(x + z * verticesNum.x)] + new Vector3(0, Mathf.PerlinNoise(xSample, zSample) * maxHeight, 0);
            }
        }

        mf.sharedMesh.vertices = vertices;
        mf.sharedMesh.RecalculateNormals();

        collider.sharedMesh = mf.sharedMesh;

        return field;
    }

    GameObject MidpointDisplacementFractal(GameObject field, float maxHeight)
    {
        Vector3[] vertices = new Vector3[(int)(verticesNum.x * verticesNum.y)];

        //フィールドの頂点
        int center = Random.Range(0, (int)(fieldSize.x * fieldSize.y));

        vertices[center] = new Vector3(0, maxHeight, 0);
        int x = center % (int)verticesNum.x;
        return field;
    }

}
