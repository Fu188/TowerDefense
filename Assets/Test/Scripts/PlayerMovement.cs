using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOAD
{
    public class PlayerMovement : MonoBehaviour
    {
        public float turnSpeed = 20f;

        Animator m_Animator;
        Rigidbody m_Rigidbody;
        Vector3 m_Movement = Vector3.zero;
        Quaternion m_Rotation = Quaternion.identity;
        int state;

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            state = hasHorizontalInput || hasVerticalInput ? 1 : 0;
            m_Animator.SetInteger("State", state);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
        }

        void OnAnimatorMove()
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
    }
}