using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateMeshFromTexture : MonoBehaviour
{
    const int maxVerticesNum = 65535;

    Texture2D texture;


    // Start is called before the first frame update
    void Start()
    {
        if(texture)
        {
            if (maxVerticesNum < texture.width * texture.height)
            {
                float kake = Mathf.Sqrt(maxVerticesNum / (texture.width * texture.height));

                int w = (int)kake * texture.width;
                int h = (int)kake * texture.height;

                int vertices = new Vector3[(resolution + 1) * (resolution + 1)];
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
