using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using ooad;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Linq;

/// <summary>
/// Photon 管理类，包含 Photon 回调函数
/// </summary>
public class PhotonNetWorkManager : MonoBehaviourPunCallbacks
{
    public static PhotonNetWorkManager _instance;//单例

    #region Private Fields

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    static string gameVersion = "1";

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    public static bool isConnecting;

    #endregion

    /*public string AppIdRealtime = "5035ebaf-9638-46ab-b232-cf74e4d570bc";
    public bool isLAN = false;
    public string ip;
    public int port = 5055;
    public ConnectionProtocol protocol = ConnectionProtocol.Udp;
    public string region;*/

    private static string baseUrl = NetWorkInfo.backEndBaseUrl + "/v1";

    public static PhotonNetWorkManager GetPhotonNetWorkManagerInstance()
    {
        return _instance;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    public static List<ConnectInfo> GetConnectInfos()
    {
        string result = HttpUtil.Get(string.Format("{0}/getConnectInfos", baseUrl));
        List<ConnectInfo> connectInfos = JsonConvert.DeserializeObject<List<ConnectInfo>>(result);

        string tempKey;

        foreach (ConnectInfo ci in connectInfos)
        {
            tempKey = AESUtil.AESDecrypt(ci.GetKey());
            ci.SetKey(tempKey);
        }
        return connectInfos;
    }


    public static void Disconnect()
    {
        PhotonNetwork.Disconnect();
        isConnecting = false;
    }


    private void InitializeLocalPlayerInfo()
    {
        Player player = PhotonNetwork.LocalPlayer;
        User user = User.GetUserInstance();
        Hashtable ht = new Hashtable();
        ht.Add("character", user.GetCharacter());
        ht.Add("avatar", user.GetAvatar());
        ht.Add("level", user.GetLevel());
        /*ht.Add("cards", new List<string>());
        ht.Add("props", new List<string>());*/
        ht.Add("seat", (int)-1);
        ht.Add("isReady", false);
        player.SetCustomProperties(ht);
        OnlineController.GetOnlineControllerInstance().OnJoinedLobbyPanelControl();
        print("InitializeLocalPlayerInfo: set properties");
    }



    public static void ConnetToMaster(ConnectInfo connectInfo, ConnectionProtocol protocol = ConnectionProtocol.Udp)
    {
        string key = connectInfo.GetKey();
        string region = connectInfo.GetRegion();
        string server = connectInfo.GetServer();
        if (region == "keykeeper")
        {
            string[] ipPort = key.Split(':');
            ConnetToMaster(ipPort[0], int.Parse(ipPort[1]), protocol);
        }
        else
        {
            ConnetToMaster(key, region, server, protocol);
        }
    }



    /// <summary>Using keykeeper server.</summary>
    /// <remarks>Udp: 5055 Tcp: 4530</remarks>
    public static void ConnetToMaster(string ip, int port = 5055, ConnectionProtocol protocol = ConnectionProtocol.Udp)
    {
        Connect(false, null, ip, port, protocol);
    }


    /// <summary>Using Photon server.</summary>
    /// <remarks>
    /// <para>region: If IsNullOrEmpty(), use BestRegion, else, use a server. Default region is China</para>
    /// <para>protocol: Udp(default), Tcp, WebSocket, WebSocketSecure</para>
    /// </remarks> 
    public static void ConnetToMaster(string appIdRealtime, string region, string server, ConnectionProtocol protocol = ConnectionProtocol.Udp)
    {
        bool connectToChina = region.ToLower().Equals("cn");
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = appIdRealtime;
        // if FixedRegion IsNullOrEmpty() AND UseNameServer == true, use BestRegion. else, use a server

        //server = connectToChina ? "ns.photonengine.cn" : ""; // ns.exitgames.com
        
        //region = connectToChina ? "cn" : "";
        Connect(true, region, server, 0, protocol);
    }

    private static void Connect(bool useNameServer, string region, string server, int port, ConnectionProtocol protocol)
    {

        isConnecting = true;
        User user = User.GetUserInstance();
        PhotonNetwork.NickName = user.GetNickname();
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = useNameServer;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
        PhotonNetwork.PhotonServerSettings.AppSettings.Server = server;
        PhotonNetwork.PhotonServerSettings.AppSettings.Port = port;
        PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = protocol;


        PhotonNetwork.GameVersion = gameVersion;

        print("tring connect to photon, " + PhotonNetwork.PhotonServerSettings.AppSettings.ToStringFull());
        print("ServerAddress: " + PhotonNetwork.ServerAddress);

        PhotonNetwork.ConnectUsingSettings();

    }


    public static void JoinRoom(string roomName)
    {
        Debug.Log(" Try to JoinOrCreateRoom, roomName: " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }


    public static void JoinOrCreateRoom(string roomName, byte maxPlayerNum)
    {
        Debug.Log(" Try to JoinOrCreateRoom, roomName: " + roomName);
        //PhotonNetwork.CreateRoom(RoomManager.localRoomCard.RoomName, new RoomOptions { MaxPlayers = RoomManager.localRoomCard.MaxPlayerNum, EmptyRoomTtl = 3000 });

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayerNum
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public static void LeftLobby()
    {
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to master server.");
        //PhotonNetwork.JoinLobby();
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            print("try to join lobby");
            PhotonNetwork.JoinLobby();
            isConnecting = false;
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("call OnConnected()");
    }


    public override void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.NickName + " joined lobby. InLobby: " + PhotonNetwork.InLobby);
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
        //PhotonNetWorkManager.JoinOrCreateRoom("test1", 2);
        InitializeLocalPlayerInfo();
        RoomManager._instance.ClearChatMessage();
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("call OnCreatedRoom(), count of room: " + PhotonNetwork.CountOfRooms);


        /* Debug.Log(PhotonNetwork.NickName + " created room; "
             +" Try to join room with room name " + RoomManager.localRoomCard.GetRoomNhame());
         PhotonNetwork.JoinRoom(RoomManager.localRoomCard.GetRoomNhame());*/

    }


    public GameObject roomNamePrefab;
    public GameObject gridLayout;
    public GameObject Lobby;
    public GameObject Room;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        print("call OnRoomListUpdate()");
        for (int i = 0; i < gridLayout.transform.childCount; ++i)
        {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }
        print(roomList);
        foreach (var room in roomList)
        {
            if (room.PlayerCount == 0)
            {
                continue;
            }
            GameObject newRoom = Instantiate(roomNamePrefab);

            newRoom.GetComponentsInChildren<Text>()[0].text = room.Name;
            newRoom.GetComponentsInChildren<Text>()[1].text = "(" + room.PlayerCount + "/" + room.MaxPlayers + ")";
            if (room.PlayerCount > 1)
            {
                newRoom.GetComponentInChildren<Button>().interactable = false;
            }
            newRoom.GetComponentInChildren<Button>().onClick.AddListener(delegate ()
            {
                HomePageManager.GetHomePageManagerInstance().StartLoading();
                JoinRoom(room.Name);
            });
            newRoom.transform.SetParent(gridLayout.transform);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        ooad.LobbyController.OnCreateRoomFailed();
    }

    public override void OnLeftLobby()
    {
        Debug.Log(PhotonNetwork.NickName + " left lobby");
        base.OnLeftLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.NickName + " disconnect. Cause: " + cause);
        ooad.LobbyController.OnDisconnected();

        base.OnDisconnected(cause);
    }

}