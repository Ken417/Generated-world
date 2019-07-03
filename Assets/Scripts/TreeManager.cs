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
        fg = GameObject.Find("Field").GetComponent<FieldGenerator>();
    }

    public void SetTree()
    {
        if (!fg) return;
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

                    if (colors[v] == Color.black)
                    {
                        tree = Instantiate(palm);
                    }
                    else
                    {
                        tree = Instantiate(broadleaf);
                    }

                    Vector3 o = vertices[v] *10000;
                    o.x += Random.Range(0, 50);
                    o.z += Random.Range(0, 50);

                    Vector3 a,b,c;
                    a = vertices[v] * 10000;
                    b = vertices[v+1] * 10000;
                    c = vertices[v+res+1] * 10000;
                    Vector3 ab, bc;
                    ab = b - a;
                    bc = c - b;
                    Vector3 n = Vector3.Cross(ab,bc);
                    if (n.y<0) { n *= -1; }
                    float height = a.y - (1 / n.y) * (n.x * (o.x - a.x) + n.z(o.z - a.z));

                    pos1 = pos2 = vertices[v] * 10000;
                    pos2.x += Random.Range(0, 50);
                    pos2.z += Random.Range(0, 50);



                    tree.SetActive(true);
                    tree.transform.position = vertices[v] * 10000;
                    addPos.x = Random.Range(0, 50);
                    addPos.z = Random.Range(0, 50);
                    addPos.y = 0;
                    float angle = Vector3.Angle(tree.transform.position, vertices[v+1] * 10000);
                    angle += Vector3.Angle(tree.transform.position, vertices[v+res] * 10000);
                    angle /= 2;
                    float mag = ((tree.transform.position + addPos) - tree.transform.position).magnitude;
                    addPos += tree.transform.position;
                    addPos.y = Mathf.Tan(angle * Mathf.Deg2Rad) * mag;
                    tree.transform.position = addPos;
                    tree.transform.parent = transform;
                    pos.Add(tree.transform.position);
                }
            }
        }
    }
}
