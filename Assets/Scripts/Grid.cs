using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    protected Mesh mesh;
    protected Vector3[] vertices;
    protected Color[] colors;
    protected Vector3[] normals;
    protected Vector2[] uv;
    protected Vector2 stepSize;


    protected void CreateGrid(Vector2 resolution)
    {
        int resoX = (int)resolution.x + (resolution.x - (int)resolution.x == 0 ? 0 : 1);
        int resoY = (int)resolution.y + (resolution.y - (int)resolution.y == 0 ? 0 : 1);

        vertices = new Vector3[(resoX + 1) * (resoY + 1)];
        colors = new Color[vertices.Length];
        normals = new Vector3[vertices.Length];
        uv = new Vector2[vertices.Length];

        stepSize = Vector2.one;
        if (resolution.x > resolution.y)
        {
            stepSize.x = resolution.x / resolution.y;
        }
        else
        {
            stepSize.y = resolution.y / resolution.x;
        }

        stepSize.x /= resolution.x;
        stepSize.y /= resolution.y;
        stepSize = Vector2.one / resolution;
        for (int v = 0, y = 0; y <= resolution.y; y++)
        {
            for (int x = 0; x <= resolution.x; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize.x - 0.5f, 0f, y * stepSize.y - 0.5f);
                colors[v] = Color.green;
                normals[v] = Vector3.up;
                uv[v] = new Vector2(x * stepSize.x, y * stepSize.y);
            }
        }
        mesh = new Mesh();
        mesh.name = "Grid";
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.uv = uv;

        int[] triangles = new int[(int)(resolution.x * resolution.y * 6)];
        for (int t = 0, v = 0, y = 0; y < resolution.y; y++, v++)
        {
            for (int x = 0; x < resolution.x; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + resoX + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + resoY + 1;
                triangles[t + 5] = v + resoY + 2;
            }
        }
        mesh.triangles = triangles;
    }

    static public Mesh Create(Vector2 resolution)
    {
        int resoX = (int)resolution.x + (resolution.x - (int)resolution.x == 0 ? 0 : 1);
        int resoY = (int)resolution.y + (resolution.y - (int)resolution.y == 0 ? 0 : 1);

        Mesh result = new Mesh();
        result.name = "Grid";
        Vector3[] vertices = new Vector3[(resoX + 1) * (resoY + 1)];
        Color[] colors = new Color[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];

        Vector2 stepSize;
        stepSize = Vector2.one / resolution;
        for (int v = 0, y = 0; y <= resolution.y; y++)
        {
            for (int x = 0; x <= resolution.x; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize.x - 0.5f, 0f, y * stepSize.y - 0.5f);
                colors[v] = Color.green;
                normals[v] = Vector3.up;
                uv[v] = new Vector2(x * stepSize.x, y * stepSize.y);
            }
        }
        result.vertices = vertices;
        result.colors = colors;
        result.normals = normals;
        result.uv = uv;

        int[] triangles = new int[(int)(resolution.x * resolution.y * 6)];
        for (int t = 0, v = 0, y = 0; y < resolution.y; y++, v++)
        {
            for (int x = 0; x < resolution.x; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + resoX + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + resoY + 1;
                triangles[t + 5] = v + resoY + 2;
            }
        }
        result.triangles = triangles;
        return result;
    }
}
