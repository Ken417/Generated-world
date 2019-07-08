using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boarding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector3 pos = transform.Find("Mast1").position;
            pos.y += 20;
            GameObject.Find("Player").GetComponent<Move>().SetPosition(pos);
        }
    }
}
