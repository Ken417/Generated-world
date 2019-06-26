using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateField : MonoBehaviour
{
    [Header("生成時に使うマテリアル"),SerializeField]
    Material material = null;

    [Header("フラットシェーディング風メッシュにするか"), SerializeField]
    bool flatMesh = false;

    [Header("フィールドの大きさ"),SerializeField]
    Vector2 fieldSize = new Vector2(10, 10);

    [Header("起伏のなめらかさ"),SerializeField]
    float relief = 15f;

    [Header("高さの最大値"),SerializeField]
    float maxHeight = 10;



    // Start is called before the first frame update
    void Start()
    {
        if(!material)
        {
            material = new Material(Shader.Find("Standard"));
        }

        GameObject obj = new GameObject();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshCollider collider = obj.AddComponent<MeshCollider>();

        Vector2 verticesNum = new Vector2(fieldSize.x + 1, fieldSize.y + 1);

        Vector3[] vertices = new Vector3[(int)(verticesNum.x * verticesNum.y)];

        float seedX = Random.value * 100f;
        float seedZ = Random.value * 100f;
        for (int x = 0; x < verticesNum.x; ++x)
        {
            for (int z = 0; z < verticesNum.y; ++z)
            {
                float xSample = (x + seedX) / relief;
                float zSample = (z + seedZ) / relief;
                vertices[(int)(x + z * verticesNum.x)] = new Vector3(x, Mathf.PerlinNoise(xSample, zSample) * maxHeight, z);
            }
        }


        int triangleNum = (int)((verticesNum.x) * (fieldSize.y)) * 2;

        int[] triangles = new int[triangleNum * 3];

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

        mesh.RecalculateNormals();

        if(flatMesh)
        {
            mesh = FlatMesh.ChangeFlatMesh(mesh);
        }

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.material = material;

    }

    // Update is called once per frame
    void Update()
    {

    }



}
