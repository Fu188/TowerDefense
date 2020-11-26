using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace ooad
{


    public class Launcher : MonoBehaviourPunCallbacks
    {

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        public GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        public GameObject progressLabel;

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 2 }, null);
                OnConnectedToMaster();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                //isConnecting = ConnectToChina();
                PhotonNetwork.GameVersion = gameVersion;
                
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

       


        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 2 }, null);
                Debug.Log("JoinOrCreateRoom: DemoRoom");
                isConnecting = false;
                Debug.Log("Ping: " + PhotonNetwork.GetPing());
            }
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log("Create room successful, current room: " + PhotonNetwork.CurrentRoom + string.Format(",  {0} inroom {1}", PhotonNetwork.NickName, PhotonNetwork.InRoom));
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (progressLabel != null)
                progressLabel.SetActive(false);
            if (controlPanel != null)
                controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 2 },null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'DemoDay 1' ");
                Debug.Log("Ping: " + PhotonNetwork.GetPing());

                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("DemoDay 1");
            }
        }

        #endregion


    }
}