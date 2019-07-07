using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    GameObject palm;
    GameObject conifer;
    GameObject broadleaf;

    FieldGenerator fg;

    // Start is called before the first frame update
    void Start()
    {
        palm = transform.Find("Palm_Desktop").gameObject;
        conifer = transform.Find("Conifer_Desktop").gameObject;
        broadleaf = transform.Find("Broadleaf_Desktop").gameObject;
        var gfg = GameObject.Find("Field");
        if (gfg)
        {
            fg = GameObject.Find("Field").GetComponent<FieldGenerator>();
        }

    }

    public void SetTree()
    {
        if (!fg) { return; }

        int res = (int)Mathf.Sqrt(fg.mesh.vertexCount);
        Vector3[] vertices = fg.mesh.vertices;
        Color[] colors = fg.mesh.colors;
        Vector3 addPos = Vector3.zero;
        for (int v = 0, z = 0; z < (res-1); z++)
        {
            for (int x = 0; x < (res - 1); x++, v++)
            {
                List<Vector3> pos = new List<Vector3>();
                for (int i = 0; i< 10;i++)
                {
                    if (Random.Range(0, 10) != 0 || vertices[v].y < 0) { continue; }

                    GameObject tree;


                    Vector3 p = vertices[v] *10000;
                    p.x += Random.Range(0, 50);
                    p.z += Random.Range(0, 50);

                    Vector3 a,b,c;
                    a = vertices[v] * 10000;
                    b = vertices[v + res + 1] * 10000;


                    for (int j =0;j<2; j++)
                    {
                        c = vertices[v+ (j* res) + (1 - j)] * 10000;
                        Vector2 p2, a2, b2, c2;
                        p2 = new Vector2(p.x, p.z);
                        a2 = new Vector2(a.x, a.z);
                        b2 = new Vector2(b.x, b.z);
                        c2 = new Vector2(c.x, c.z);

                        if (PointInTriangle(p2, a2, b2, c2))
                        {
                            Vector3 ab, bc;
                            ab = b - a;
                            bc = c - b;
                            Vector3 n = Vector3.Cross(ab, bc);
                            if (n.y < 0) { n *= -1; }
                            p.y = a.y - (1 / n.y) * (n.x * (p.x - a.x) + n.z * (p.z - a.z));

                            if(p.y>0)
                            {
                                if (colors[v] == Color.black)
                                {
                                    tree = Instantiate(palm);
                                }
                                else
                                {
                                    tree = Instantiate(broadleaf);
                                }
                                tree.SetActive(true);
                                tree.transform.position = p;
                                tree.transform.parent = transform;
                                pos.Add(p);
                            }

                            break;
                        }
                    }
                }
            }
        }
        fg = null;
    }

    float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return(v1.x* v2.y -v1.y * v2.x);
    }

    bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b , Vector2 c)
    {
        bool b1 = Vec2Cross(c - b, p - c) > 0.0f;
        bool b2 = Vec2Cross(b - a, p - b) > 0.0f;
        bool b3 = Vec2Cross(a - c, p - a) > 0.0f;

        return (b1 == b2) && (b2 == b3);
    }
}
