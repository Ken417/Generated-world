using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CreateMeshFromTexture : Grid
{
    [Header("頂点の最大数"),Range(4, 65535), SerializeField]
    int maxVerticesNum = 65535;

    [SerializeField]
    Texture2D texture= null;

    [SerializeField]
    Shader shader;

    [Header("フィールドの高さ"), SerializeField]
    float height = 1;

    [Header("フィールドの色"), SerializeField]
    Gradient coloring;


    int w, h;
    float step = 1;

    Color[] pix;

    // Start is called before the first frame update
    void Awake()
    {
        if (!texture)
        {
            texture = TextureGenerator.CreateUnityPerlinNoise2DTexture(200, 200, Vector2.zero, 0, 3, 5, 2.85f, 0.483f);
        }
        Create(texture);
    }

    public void Create(Texture2D tex)
    {
        if (maxVerticesNum < texture.width * texture.height)
        {
            step = Mathf.Sqrt((float)maxVerticesNum / (texture.width * texture.height));

            w = (int)(step * texture.width);
            h = (int)(step * texture.height);
            if (w < 2) { w = 2; }
            if (h < 2) { h = 2; }
        }
        else
        {
            w = texture.width;
            h = texture.height;
        }

        pix = texture.GetPixels();

        CreateGrid(new Vector2((w - 1), (h - 1)));

        Refresh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        if (!shader)
        {
            shader = Shader.Find("Standard");
        }
        GetComponent<MeshRenderer>().material = new Material(shader);
    }


    void Refresh()
    {
        for (int v = 0, z = 0; z < h; z++)
        {
            for (int x = 0; x < w; x++, v++)
            {
                int c = (int)((texture.width * (texture.height / h) * z) + (x * ((float)texture.width / w)));
                float he = pix[c].r;
                he -= 0.5f;
                vertices[v] = new Vector3(x * stepSize.x - 0.5f, he * step * height, z * stepSize.y - 0.5f);
                colors[v] = coloring.Evaluate(he);
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }
}
