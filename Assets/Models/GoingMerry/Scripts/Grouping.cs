using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grouping : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject unnecessary = GroupBy("Unnecessary");
        unnecessary.transform.parent = transform;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (!t.GetComponent<MeshRenderer>())
            {
                t.parent = unnecessary.transform;
                //Destroy(t.gameObject);
            }
        }
        transform.Find("skp_camera_Last_Saved_SketchUp_View").parent = unnecessary.transform;
        transform.Find("Plane").parent = unnecessary.transform;
        unnecessary.SetActive(false);
        Destroy(unnecessary);

        Transform hull = GroupBy("Hull").transform;
        Transform mast1 = GroupBy("Mast1").transform;
        Transform mast2 = GroupBy("Mast2").transform;
        GroupBy("Sail1").transform.parent = mast1;
        GroupBy("Sail2").transform.parent = mast2;
        GroupBy("Flag1").transform.parent = mast1;
        GroupBy("Flag2").transform.parent = mast2;
        GroupBy("Anchor");
        GroupBy("Rudder");
        GroupBy("Main cannon");
        GroupBy("Sub cannon");
        GroupBy("Rudder");
        Transform deco = GroupBy("Decoration").transform;
        GroupBy("Bow rail").transform.parent = deco;
        GroupBy("Stern rail").transform.parent = deco;
        GroupBy("Stair").transform.parent = deco;
        GroupBy("Mandarin orange").transform.parent = GroupBy("Cabin").transform;
        GroupBy("Bow").transform.parent = hull;
        GroupBy("Midship").transform.parent = hull;
        GroupBy("Stern").transform.parent = hull;
        GroupBy("Deck").transform.parent = hull;
        GroupBy("Transom").transform.parent = hull;
    }

    GameObject GroupBy(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
            {
                t.parent = go.transform;
            }
        }
        return go;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
