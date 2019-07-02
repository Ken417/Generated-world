using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]//自動でコンポーネントをアタッチしてくれる
public class Move : MonoBehaviour
{
    [Header("初期位置"), SerializeField]
    Vector3 initPos = new Vector3(0, 100, 0);

    [Header("歩く速さ"), SerializeField]
    float walkSpeed = 6.0f;

    [Header("ジャンプ力"), SerializeField]
    float jumpPower = 8.0f;

    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    void OnEnable()
    {
        transform.position = initPos;
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpPower;

        }
        moveDirection += Physics.gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
