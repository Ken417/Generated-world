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

    [Header("走りの係数"), SerializeField]
    float runCoefficient = 2.0f;

    [Header("ジャンプ力"), SerializeField]
    float jumpPower = 8.0f;

    [Header("重さ"), SerializeField]
    float weight = 1.0f;

    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    Vector3 setPos = Vector3.zero;
    bool set = false;

    void OnEnable()
    {
        transform.position = initPos;
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!set)
        {
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                if (Input.GetKey(KeyCode.LeftShift)) { moveDirection *= walkSpeed * runCoefficient; }
                else { moveDirection *= walkSpeed; }
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpPower;

            }
            moveDirection += Physics.gravity * weight;
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            set = false;
            transform.position = setPos;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        set = true;
        setPos = pos;
    }
}
