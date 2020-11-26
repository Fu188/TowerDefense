using UnityEngine;

namespace ooad
{

    public class PlayerManagerLocal1 : MonoBehaviour
    {

        private CharacterController Controller;
        private Animator Animator;

        public float RotateSpeed = 0.2f;

        private float HorizontalMove, VerticalMove, UpMove; // 获取按键值
        private Vector3 MoveDir; // Player移动方向

        private bool IsGround;
        private bool IsFiring;
        private bool IsFlying;
        private bool IsFlyPeak;




        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {

            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();

            if (!Controller)
            {
                Debug.LogError("Player CharacterController is Missing Controller Component", this);
            }
            else
            {
                Debug.Log("CharacterController loaded: " + Controller.name + "  id: " + Controller.GetInstanceID());
            }
            if (!Animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }

        }


        void Update()
        {

            ProcessInputs();
            PlayerMoveControl();
            PlayerAnimatorControl();

        }



        bool CheckGround()
        {
            return Physics.Raycast(transform.localPosition, -Vector3.up, 0.2f); // Controller.isGrounded || 
        }

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {

            IsGround = CheckGround();

            IsFlying = Input.GetButton("Jump");

            IsFlyPeak = !Physics.Raycast(transform.localPosition, -Vector3.up, 3f);

            
            if (IsFlying)
            {
                if (IsFlyPeak)
                {
                    UpMove = 0;
                }
                else
                {
                    UpMove = 3;
                }
                
            }
            else
            {
                if (IsGround)
                {
                    UpMove = -0.5f;

                }
                else
                {
                    UpMove = -3;
                }

            }

            VerticalMove =  Input.GetAxis("Vertical");
            VerticalMove = VerticalMove > 0 ? VerticalMove * 4 : VerticalMove;
            HorizontalMove = Input.GetAxis("Horizontal");


            if (IsGround)
            {
                IsFiring = Input.GetButton("Fire1");
                /*if (Input.GetButtonDown("Fire1"))
                {
                    if (!IsFiring)
                    {
                        IsFiring = true;
                    }
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    if (IsFiring)
                    {
                        IsFiring = false;
                    }
                }*/
            }


            /*bool hasHorizontalInput = !Mathf.Approximately(HorizontalMove, 0f);
            bool hasVerticalInput = !Mathf.Approximately(VerticalMove, 0f);*/

        }


        private void PlayerMoveControl()
        {

            MoveDir = transform.forward * VerticalMove + transform.up * UpMove;
            
            Controller.Move(MoveDir * Time.deltaTime);

            //主角的朝向即为下一个点坐标减去主角坐标的向量

            //插值改变主角的朝向，使其有一个自然转向的过程，防止其瞬间转向
            //Controller.transform.forward = Vector3.Lerp(transform.forward, transform.forward+new Vector3( 0.1f,0), 0.01f);
            if(HorizontalMove > 0)
            {
                Controller.transform.Rotate(Vector3.up * RotateSpeed);
               
            }
            else if (HorizontalMove < 0)
            {
                Controller.transform.Rotate(Vector3.up * (-RotateSpeed));
                
            }
            
        }


        private void PlayerAnimatorControl()
        {
            bool NotInAir = Physics.Raycast(transform.localPosition, -Vector3.up, 0.15f);
            Animator.SetBool("RunFront", VerticalMove > 0 && NotInAir);
            Animator.SetBool("WalkBack", VerticalMove < 0 && NotInAir);
            Animator.SetBool("Shoot", IsFiring);

            if (VerticalMove != 0 || IsFiring || IsFlying)
            {
                Animator.SetBool("IdlePlus", false);
            }
            else
            {
                Animator.SetBool("IdlePlus", true);
            }

        }



    }

}
