using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSways : MonoBehaviour
{
    Quaternion startRot;
    Quaternion goalRot1;
    Quaternion goalRot2;

    float randRotRange = 10;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
        goalRot1 = Quaternion.Euler(startRot.eulerAngles.y + Random.Range(0f, randRotRange), startRot.eulerAngles.y, startRot.eulerAngles.y + Random.Range(0f, randRotRange));
        goalRot2 = Quaternion.Euler(startRot.eulerAngles.y + Random.Range(-randRotRange, 0f), startRot.eulerAngles.y, startRot.eulerAngles.y + Random.Range(-randRotRange, 0f));
        goalRot1 = Quaternion.Lerp(goalRot1, startRot, 0.9f);
        goalRot2 = Quaternion.Lerp(goalRot2, startRot, 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(goalRot1, goalRot2, (Mathf.Sin(Time.time)+1)/2);
    }
}
