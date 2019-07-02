using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    enum CameraMode
    {
        FPS,
        TPS,
    }

    [Header("カメラのモード"), SerializeField]
    CameraMode cameraMode = CameraMode.FPS;
    CameraMode currentCameraMode = cameraMode;

    GameObject character;

    Vector3 tpsCameraPos = new Vector3(0, 1.5f, -4f);
    float tpsCameraAngle = 3;



    // Use this for initialization
    void Start()
    {
        character = transform.parent.parent.Find("Character").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.parent.Rotate(0, Input.GetAxis("Mouse X"), 0);
        character.transform.Rotate(0, -Input.GetAxis("Mouse X"), 0);
        transform.parent.Rotate(Input.GetAxis("Mouse Y") * -1, 0, 0);

        if (currentCameraMode != cameraMode)
        {
            currentCameraMode = cameraMode;
            transform.localRotation = Quaternion.identity;
            transform.localPosition += (transform.parent.forward) * tpsPos * cameraMoveVec;
            cameraMoveVec *= -1;
        }
    }
    //float tpsPos = 2;
    //int cameraMoveVec = -1;


    //void Start()
    //{

    //}

    //void Update()
    //{

    //    transform.parent.Rotate(0, Input.GetAxis("Mouse X"), 0);
    //    if(cameraMoveVec>0)
    //    {
    //        transform.parent.Rotate(Input.GetAxis("Mouse Y") * -1, 0, 0);
    //    }
    //    else
    //    {
    //        transform.Rotate(Input.GetAxis("Mouse Y") * -1, 0, 0);
    //    }
    //    if (Input.GetKeyDown(KeyCode.F5))
    //    {
    //        transform.localRotation = Quaternion.identity;
    //        transform.localPosition += (transform.parent.forward) * tpsPos * cameraMoveVec;
    //        cameraMoveVec *= -1;
    //    }
    //}
}
