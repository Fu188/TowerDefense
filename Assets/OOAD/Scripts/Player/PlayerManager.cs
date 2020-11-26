using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;


namespace ooad
{

    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {

       

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public GameObject LocalPlayerCamera;

        /*[Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;*/


        private CharacterController Controller;
        private Animator Animator;

        private float HorizontalMove, VerticalMove, UpMove; // 获取按键值
        private Vector3 MoveDir; // Player移动方向

        private bool IsGround;
        private bool IsFiring;
        private bool IsFlying;
        private bool IsFlyPeak;


        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }




        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;
                LocalPlayerCamera.SetActive(true);

            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

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


            /*CameraManager _cameraManager = this.gameObject.GetComponent<CameraManager>();

            if (_cameraManager != null)
            {
                if (photonView.IsMine)
                {
                    _cameraManager.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraManager Component on playerPrefab.", this);
            }
*/

        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {

            if (photonView.IsMine)
            {
                ProcessInputs();
                PlayerMoveControl();
                PlayerAnimatorControl();
            }


            /*// trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }*/
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

            VerticalMove = HorizontalMove == 0 ? Input.GetAxis("Vertical") : 0;
            VerticalMove = VerticalMove > 0 ? VerticalMove * 4 : VerticalMove;
            HorizontalMove = VerticalMove == 0 ? Input.GetAxis("Horizontal") : 0;


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

            MoveDir = transform.forward * VerticalMove + transform.right * HorizontalMove + transform.up * UpMove;
            Controller.Move(MoveDir * Time.deltaTime);
        }


        private void PlayerAnimatorControl()
        {
            bool NotInAir = Physics.Raycast(transform.localPosition, -Vector3.up, 0.15f);
            Animator.SetBool("RunFront", VerticalMove > 0 && NotInAir);
            Animator.SetBool("WalkBack", VerticalMove < 0 && NotInAir);
            Animator.SetBool("WalkRight", HorizontalMove > 0 && NotInAir);
            Animator.SetBool("WalkLeft", HorizontalMove < 0 && NotInAir);
            Animator.SetBool("Shoot", IsFiring);

            if (HorizontalMove != 0 || VerticalMove != 0 || IsFiring || IsFlying)
            {
                Animator.SetBool("IdlePlus", false);
            }
            else
            {
                Animator.SetBool("IdlePlus", true);
            }
        }


        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
       /* void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;
            Debug.Log("Hit--> Health = " + Health);
        }*/
        
        
        
        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// We're going to affect health while the beams are touching the player
        /// </summary>
        /// <param name="other">Other.</param>
        /*void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            Health -= 0.1f * Time.deltaTime;
            Debug.Log("Hit--> Health = " + Health);
        }*/


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            /*if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }*/

            /*GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);*/
        }
        



        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            /*if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
                Debug.Log("SendNext--> Health = " + Health);
            }
            else
            {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
                Debug.Log("ReceiveNext--> Health = " + Health);
            }*/
        }


        #endregion



        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    }

}
