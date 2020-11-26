using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class KKHeroManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static KKHeroManager _instance;


    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    private static GameObject LocalPlayerInstance;

    [Tooltip("Control the rotation speed of the hero")]
    public float RotateSpeed = 1.5f;

    [Tooltip("Hero movement speed")]
    public float MoveSpeed = 1;

    [Tooltip("When the hero y coordinate is less than ResetPostitionY, the hero position is reset")]
    public float ResetPostitionY = -10;


    private CharacterController Controller;
    private Animator Animator;

    private float HorizontalMove, VerticalMove, UpMove; // 获取按键值
    private Vector3 MoveDir; // Hero 移动方向

    private bool IsGround;
    //private bool IsFiring;
    private bool IsFlying;
    private bool IsFlyPeak;
    private Vector3 HeroScaleOrigin;


    private Vector3 HomePosition;
    private Vector3 InitialPosition;
    private CountDownTimer speedTimer;
    private bool backHomeCache;

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if ((!PhotonNetwork.IsConnected) || photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
            MiniMapController.Instance.target = LocalPlayerInstance.transform;
            HeroScaleOrigin = LocalPlayerInstance.transform.localScale;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        //DontDestroyOnLoad(this.gameObject);
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        KKHeroCameraManager _cameraManager = this.gameObject.GetComponent<KKHeroCameraManager>();
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        if ((!PhotonNetwork.IsConnected) || photonView.IsMine)
        {
            GameManager.peopleTransform = transform;
        }



        if (_cameraManager != null)
        {
            if ((!PhotonNetwork.IsConnected) || photonView.IsMine)
            {
                _cameraManager.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> KKheroCameraManager Component on KKheroPrefab.", this);
        }

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
            Debug.LogError("KKHeroManager is Missing Animator Component", this);
        }

        List<Vector3> waypoints = GameManager.Instance.levelStuffFromXML.Waypoints;
        int pathCount = waypoints.Count;
        //transform.position = waypoints[0];

        HomePosition = waypoints[pathCount - 2];

    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {

        if ((!PhotonNetwork.IsConnected) || photonView.IsMine)
        {
            ProcessInputs();
            if (transform.position.y < ResetPostitionY || Input.GetKeyDown(KeyCode.P))
            {
                backHomeCache = true;
            }
            if (backHomeCache)
            {
                backHomeCache = false;
                LocalPlayerInstance.transform.position = HomePosition;
            }
            else
            {
                PlayerMoveControl();
            }


            PlayerAnimatorControl();

        }
    }


    public static KKHeroManager GetKKHeroManagerInstance()
    {
        return _instance;
    }




    /// <summary>
    /// hero 移动速度加倍，一段时间后失效
    /// </summary>
    /// <param name="mul">速度倍数</param>
    /// <param name="time">生效时间 单位 秒</param>
    public void MulHeroSpeed(float mul, float time)
    {
        MoveSpeed *= mul;
        Invoke("ResetHeroSpeed", time);
    }

    /// <summary>
    /// hero 移动速度加倍，效果生效至 ResetHeroSpeed() 被执行
    /// </summary>
    /// <param name="mul">速度倍数</param>
    public void MulHeroSpeed(float mul)
    {
        MoveSpeed *= mul;
    }

    /// <summary>
    /// 重置 hero 移动速度
    /// </summary>
    public void ResetHeroSpeed()
    {
        MoveSpeed = 1;
    }

    public void BiggerHero(float mul, float time)
    {
        transform.localScale *= mul;
        MulHeroSpeed(mul);
        Invoke("ResetHeroScale", time);
    }

    public void ResetHeroScale()
    {
        transform.localScale = HeroScaleOrigin;
        ResetHeroSpeed();
    }

    /// <summary>
    /// 重置 hero 的位置 至 加载场景时的初始位置
    /// </summary>
    public void BackHome()
    {
        backHomeCache = true;
    }


    bool CheckGround()
    {
        return Controller.isGrounded;//Physics.Raycast(transform.localPosition, -Vector3.up, 0.01f); // Controller.isGrounded || 
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

        VerticalMove = Input.GetAxis("Vertical");
        VerticalMove = VerticalMove > 0 ? VerticalMove * 4 : VerticalMove;
        HorizontalMove = Input.GetAxis("Horizontal");


        /*if (IsGround)
        {
            IsFiring = Input.GetButton("Fire1");
            *//*if (Input.GetButtonDown("Fire1"))
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
            }*//*
        }*/


        /*bool hasHorizontalInput = !Mathf.Approximately(HorizontalMove, 0f);
        bool hasVerticalInput = !Mathf.Approximately(VerticalMove, 0f);*/

    }


    private void PlayerMoveControl()
    {

        MoveDir = transform.forward * VerticalMove + transform.up * UpMove;

        Controller.Move(MoveDir * Time.deltaTime * MoveSpeed);

        //主角的朝向即为下一个点坐标减去主角坐标的向量

        //插值改变主角的朝向，使其有一个自然转向的过程，防止其瞬间转向
        //Controller.transform.forward = Vector3.Lerp(transform.forward, transform.forward+new Vector3( 0.1f,0), 0.01f);
        if (HorizontalMove > 0)
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
        bool NotInAir = CheckGround(); // Physics.Raycast(transform.localPosition, -Vector3.up, 0.15f);
        Animator.SetBool("RunFront", VerticalMove > 0 && NotInAir);
        Animator.SetBool("WalkBack", VerticalMove < 0 && NotInAir);
        //Animator.SetBool("Shoot", IsFiring);

        if (HorizontalMove != 0 || VerticalMove != 0 /*|| IsFiring*/ || IsFlying)
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
}

