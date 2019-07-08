using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingMerryManager : MonoBehaviour
{
    Rudder rudder;

    // Start is called before the first frame update
    void Start()
    {
        rudder = transform.Find("Rudder").GetComponent<Rudder>();
    }

    // Update is called once per frame
    void Update()
    {
        //rudder.SetAngle(Random.Range(-10,10));
    }
}
