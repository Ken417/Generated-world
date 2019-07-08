using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
    GameObject hinge;
    // Start is called before the first frame update
    void Start()
    {
        hinge = transform.Find("Hinge").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAngle(float angle)
    {
        hinge.transform.localRotation = Quaternion.Euler(0,0,angle);
    }
}
