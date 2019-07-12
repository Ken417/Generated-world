using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CreateMeshFromTexture : MonoBehaviour
{
    const int maxVerticesNum = 65534;

    [SerializeField]
    Texture2D texture;

    [Header("フィールドの高さ"), SerializeField]
    float height = 1;

    [Header("フィールドの色"), SerializeField]
    Gradient coloring;

    Mesh mesh;


    int w, h;
    float kake = 1;
    float stepSizeW, stepSizeH;

    Vector3[] vertices;
    Color[] colors;
    Color[] pix;

    // Start is called before the first frame update
    void Start()
    {
        texture = TextureGenerator.CreateUnityPerlinNoise2DTexture(200,200,Vector2.zero,0,3,5,2.85f,0.483f);
        if(texture)
        {

            if (maxVerticesNum < texture.width * texture.height)
            {
                kake = Mathf.Sqrt((float)maxVerticesNum / (texture.width * texture.height));

                w = (int)(kake * texture.width);
                h = (int)(kake * texture.height);
            }
            else
            {
                w = texture.width;
                h = texture.height;
            }

            print("texture.width=" + texture.width);
            print("texture.height=" + texture.height);
            print("kake=" + kake);
            print("w=" + w);
            print("h=" + h);

            pix = texture.GetPixels();

            vertices = new Vector3[w * h];
            colors = new Color[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];
            Vector2[] uv = new Vector2[vertices.Length];
            if (texture.width > texture.height)
            {
                stepSizeH = 1.0f;
                stepSizeW = (float)texture.width / texture.height;
            }
            else
            {
                stepSizeH = (float)texture.height/texture.width;
                stepSizeW = 1.0f;
            }

            stepSizeW /= w;
            stepSizeH /= h;
            for (int v = 0, z = 0; z < h; z++)
            {
                for (int x = 0; x < w; x++, v++)
                {
                    int c = (int)((texture.width * (texture.height/h) * z) + (x * ((float)texture.width / w)));
                    float he = pix[c].r;
                    //if(he<=0.1f)
                    //{
                    //    he = Mathf.PerlinNoise(((float)x/w)*100,((float)z /h)*100)-1;
                    //}
                    //he *= 1.2f;
                    //he -= 0.5f;
                    vertices[v] = new Vector3(x * stepSizeW - 0.5f, he*kake * height, z * stepSizeH - 0.5f);
                    colors[v] = coloring.Evaluate(he);
                    //colors[v] = coloring.Evaluate((he+1f)/2);
                    normals[v] = Vector3.up;
                    uv[v] = new Vector2(x * stepSizeW, z * stepSizeH);
                }
            }
            mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.normals = normals;
            mesh.uv = uv;

            int[] triangles = new int[(w ) * (h) * 6];
            for (int t = 0, v = 0, y = 0; y < (h - 1); y++, v++)
            {
                for (int x = 0; x < (w - 1); x++, v++, t += 6)
                {
                    triangles[t] = v;
                    triangles[t + 1] = v + (w - 1) + 1;
                    triangles[t + 2] = v + 1;
                    triangles[t + 3] = v + 1;
                    triangles[t + 4] = v + (w - 1) + 1;
                    triangles[t + 5] = v + (w - 1) + 2;
                }
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().sharedMesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Surface Shader"));
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int v = 0, z = 0; z < h; z++)
        {
            for (int x = 0; x < w; x++, v++)
            {

                int c = (int)((texture.width * (texture.height / h) * z) + (x * ((float)texture.width / w)));
                float he = pix[c].r;
                //if (he <= 0.1f)
                //{
                //    he = Mathf.PerlinNoise(((float)x/w)*100,((float)z /h)*100)-1;
                //    he *= 0.3f;
                //}
                //he *= 1.2f;
                //he -= 0.5f;
                vertices[v] = new Vector3(x * stepSizeW - 0.5f, he * kake * height, z * stepSizeH - 0.5f);
            }
        }
        mesh.vertices = vertices;
    }
}
