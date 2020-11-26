using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed;
    public float jumpSpeed;

    private float horizontalMove, verticalMove; // 获取按键值
    private Vector3 dir; // Player移动方向

    public float gravity;
    private Vector3 velocity; // 控制Y轴加速度

    private bool isGround;

    Animator m_Animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        GameManager.peopleTransform = transform;
        // 检测地面
        isGround = controller.isGrounded;

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMove = Input.GetAxis("Vertical") * moveSpeed;

        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        controller.Move(dir * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = jumpSpeed;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move((dir+velocity) * Time.deltaTime);

        // 更改动画状态
        bool hasHorizontalInput = !Mathf.Approximately(horizontalMove, 0f);
        bool hasVerticalInput = !Mathf.Approximately(verticalMove, 0f);
        int state = hasHorizontalInput || hasVerticalInput ? 1 : 0;
        m_Animator.SetInteger("State", state);
    }
}