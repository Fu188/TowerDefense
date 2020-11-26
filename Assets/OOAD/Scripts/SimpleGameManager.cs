using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SimpleGameManager : MonoBehaviourPunCallbacks
{
    public static SimpleGameManager Instance;
    private bool isConnecting;
    //public  string levelName = "test";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            print("already connected");
            if (!PhotonNetwork.InRoom)
            {
                Debug.Log("JoinOrCreateRoom: DemoRoom");
                PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 4 }, null);
            }
            
        }
        else
        {
            print("try to connect to photon.");
            // #Critical, we must first and foremost connect to Photon Online Server.
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            //isConnecting = PhotonNetwork.ConnectUsingSettings();
            isConnecting = ConnectToChina();
            PhotonNetwork.GameVersion = "1";

        }
    }


    bool ConnectToChina()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cn";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "5035ebaf-9638-46ab-b232-cf74e4d570bc";
        PhotonNetwork.PhotonServerSettings.AppSettings.Server = "ns.photonengine.cn";
        return PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 4 }, null);
            Debug.Log("JoinOrCreateRoom: DemoRoom");
            isConnecting = false;
            Debug.Log("Ping: " + PhotonNetwork.GetPing());
        }
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Join room successful");
        //PhotonNetwork.LoadLevel(levelName);
        PhotonNetwork.Instantiate(Resources.Load("KKhero1").name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. Try JoinOrCreateRoom again");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 4 }, null);
    }



}
