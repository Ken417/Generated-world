using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    Move move;
    Camera camera;

    [Header("Move Script on/off"), SerializeField]
    bool useMoveScript = true;


    [Header("Camera Script on/off"), SerializeField]
    bool useCameraScript = true;

    void Start()
    {
        move = GetComponent<Move>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move.enabled != useMoveScript)
        {
            move.enabled = useMoveScript;
        }
        if (camera.enabled != useCameraScript)
        {
            camera.enabled = useCameraScript;
        }
        
    }
}
