using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Networking;

/// <summary>
/// RoomManager in Lobby V2.0
/// </summary>
public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static RoomManager _instance;//单例

    public GameObject dungeonBtn;//选择副本按钮
    public GameObject prepareBtn;//准备卡牌道具按钮
    public GameObject readyBtn;//准备按钮
    public GameObject startBtn;//开始游戏按钮

    object RoomGameInfo;
    public GameObject PlayersPanel;
    public GameObject OnlinePanel;
    Hashtable roomInfoCache;
    bool cachingRoomInfo;
    bool startUpdateInfo;
    public static bool entering;

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }

    }

    private void Start()
    {
        cachingRoomInfo = false;
        startUpdateInfo = false;
        entering = false;
    }


    #region Player events function
    public void OnClickReadyBtn()
    {
        bool isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"];
        LocalReady(!isReady);
    }
    public void LocalReady(bool ready)
    {
        Hashtable ht = new Hashtable();
        ht.Add("isReady", ready);


        bool[] readyList = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["readyList"];
        readyList[(int)PhotonNetwork.LocalPlayer.CustomProperties["seat"]] = ready;
        roomInfoCache = new Hashtable
            {
                { "readyList", readyList }
            };
        cachingRoomInfo = true;

        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    public void ChangeDungeon(string dungeon)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Hashtable ht = new Hashtable
                {
                    { "dungeon", dungeon }
                };
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
    }

    public void StartGame()
    {
        if (!entering)
        {
            PhotonNetwork.LoadLevel("InGame");
            entering = true;
            UserInfoManager.UpdateUserHairAndTime(-5);
        }
    }

    /// <summary>
    /// The player exits the room, places it on a button event
    /// </summary>
    public void LeftRoom()
    {
        LocalReady(false);
        //int position = (int)PhotonNetwork.LocalPlayer.CustomProperties["seat"];
        //ReleaseSeat(position);
        PhotonNetwork.LeaveRoom();
        OnlinePanel.transform.GetChild(0).gameObject.SetActive(true);
        OnlinePanel.transform.GetChild(1).gameObject.SetActive(false);
    }
    #endregion


    #region Room events function
    public void UpdateRoomPlayersInfo()
    {
        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
        print("UpdateRoomPlayersInfo: players count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        foreach (KeyValuePair<int, Player> kvp in players)
        {
            print("Player  <" + kvp.Value.NickName + ">  CustomProperties: " + kvp.Value.CustomProperties.ToStringFull());
            //  添加 每一位用户信息
            Player player = kvp.Value;
            string nickName = player.NickName;
            int level = (int)player.CustomProperties["level"];
            string avatar = (string)player.CustomProperties["avatar"];
            int position = (int)player.CustomProperties["seat"];
            bool isReady = (bool)player.CustomProperties["isReady"];
            GameObject PlayerPanel = PlayersPanel.transform.GetChild(position).gameObject;
            print("get PlayerPanel, PlayerPanel name: " + PlayerPanel.name);
            for (int i = 0; i < PlayerPanel.transform.childCount; i++)
            {
                PlayerPanel.transform.GetChild(i).gameObject.SetActive(true);
            }
            ShowImage(PlayerPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>(), avatar);
            PlayerPanel.GetComponentsInChildren<Text>()[0].text = nickName;
            PlayerPanel.GetComponentsInChildren<Text>()[1].text = "Lv. " + level;
            //PlayerPanel.GetComponentInChildren<Image>().sprite;
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PlayerPanel.GetComponentsInChildren<Text>()[0].color = new Color32(73, 84, 255, 255);
                PlayerPanel.GetComponentsInChildren<Text>()[1].color = new Color32(73, 84, 255, 255);
                if (isReady)
                {
                    readyBtn.gameObject.GetComponentInChildren<Text>().text = "Cancel";
                    prepareBtn.GetComponent<Button>().interactable = false;
                }
                else
                {
                    readyBtn.gameObject.GetComponentInChildren<Text>().text = "Ready";
                    prepareBtn.GetComponent<Button>().interactable = true;
                }
            }
            if (isReady)
            {
                PlayerPanel.GetComponent<Image>().color = new Color32(115, 255, 182, 192);
            }
            else
            {
                PlayerPanel.GetComponent<Image>().color = new Color32(26, 26, 26, 192);
            }
            if (player.ActorNumber == PhotonNetwork.CurrentRoom.masterClientId)
            {
                PlayerPanel.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                PlayerPanel.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.CurrentRoom.masterClientId)
        {
            prepareBtn.GetComponent<Button>().interactable = true;
        }

        bool[] occupyList = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["seatEmpty"];
        for (int i = 0; i < occupyList.Length; i++)
        {
            if (occupyList[i])
            {
                for (int j = 0; j < PlayersPanel.transform.GetChild(i).childCount; j++)
                {
                    PlayersPanel.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
                PlayersPanel.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(26, 26, 26, 192);
            }
        }
        OnlineController.GetOnlineControllerInstance().OnJoinedRoomPanelControl();
    }
    #endregion

    private int ApplySeat()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        bool[] seatEmpty = (bool[])ht["seatEmpty"];
        for (int i = 0; i < seatEmpty.Length; ++i)
        {
            if (seatEmpty[i])
            {
                seatEmpty[i] = false;
                Hashtable newHt = new Hashtable
                    {
                        { "seatEmpty", seatEmpty }
                    };
                roomInfoCache = newHt;
                cachingRoomInfo = true;
                return i;
            }
        }
        return -1;
    }

    private void ReleaseSeat(int seatNum)
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        bool[] seatEmpty = (bool[])ht["seatEmpty"];
        bool[] readyList = (bool[])ht["readyList"];
        seatEmpty[seatNum] = true;
        readyList[seatNum] = false;
        roomInfoCache = new Hashtable
            {
                { "seatEmpty", seatEmpty },
                { "readyList", readyList }
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomInfoCache);
        //cachingRoomInfo = true;
    }


    private bool CheckAllReady()
    {
        bool[] readyList = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["readyList"];
        foreach (bool ready in readyList)
        {
            if (!ready)
            {
                return false;
            }
        }

        return true;
    }
    public void MasterClientDooo()
    {
        // TODO 判断是否是房主，是就显示开始游戏按钮，不是就显示准备按钮;房主才可选择副本
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            dungeonBtn.GetComponent<Button>().interactable = true;
            readyBtn.SetActive(false);
            startBtn.SetActive(true);
            //PhotonNetwork.LoadLevel("DemoDay 1");
        }
        else
        {
            dungeonBtn.GetComponent<Button>().interactable = false;
            readyBtn.SetActive(true);
            startBtn.SetActive(false);
        }
    }

    #region showUrlImage
    public void ShowImage(Image image, string Url)
    {
        StartCoroutine(GetFullSprite(image, Url));
    }

    IEnumerator GetFullSprite(Image image, string Url)
    {
        yield return StartCoroutine(GetTexture(image, Url));
    }
    /// <summary>
    /// 通过Url获取到Image
    /// </summary>
    /// <param name="Url"></param>
    /// <returns></returns>
    IEnumerator GetTexture(Image image, string Url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url);
        print(Url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            print(texture);
        }
    }
    #endregion


    #region Photon Callback Funtion


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        print("RoomManager: call OnRoomPropertiesUpdate, propertiesThatChanged: " + propertiesThatChanged.ToStringFull());
        if (propertiesThatChanged.ContainsKey("readyList") && CheckAllReady())
        {
            startBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            startBtn.GetComponent<Button>().interactable = false;
        }
        if (startUpdateInfo)
        {
            UpdateRoomPlayersInfo();
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        print("RoomManager: OnPlayerPropertiesUpdate was called, player " + targetPlayer.NickName + " changedProps: " + changedProps.ToStringFull());
        startUpdateInfo = true;

        if (cachingRoomInfo)
        {
            print("RoomManager: call CurrentRoom.SetCustomProperties");
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomInfoCache);
            }

            cachingRoomInfo = false;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        cachingRoomInfo = false;
        startUpdateInfo = false;
        Debug.Log("RoomManager: call OnJoinedRoom(), count of player: " + PhotonNetwork.CurrentRoom.PlayerCount);
        MasterClientDooo();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            print("room master client Initialize room information, room name: " + PhotonNetwork.CurrentRoom.Name);
            roomInfoCache = new Hashtable();
            roomInfoCache.Add("dungeon", "default");
            bool[] readyList = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];
            bool[] seatEmpty = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];
            readyList[0] = true;
            for (int i = 1; i < seatEmpty.Length; ++i)
            {
                seatEmpty[i] = true;
            }

            roomInfoCache.Add("readyList", readyList);
            roomInfoCache.Add("seatEmpty", seatEmpty);
            cachingRoomInfo = true;
            Hashtable ht = new Hashtable();
            ht.Add("seat", 0);
            ht.Add("isReady", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        }
        /*else
        {
            Hashtable ht = new Hashtable();
            ht.Add("seat", ApplySeat());
            PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        }*/

    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print("RoomManager: call OnPlayerEnteredRoom, " + newPlayer.NickName + " enter room");
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Hashtable ht = new Hashtable();
            ht.Add("seat", ApplySeat());
            newPlayer.SetCustomProperties(ht);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print("RoomManager: call OnPlayerLeftRoom, " + otherPlayer.NickName + " left room");

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            print((int)otherPlayer.CustomProperties["seat"]);
            ReleaseSeat((int)otherPlayer.CustomProperties["seat"]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        print("OnMasterClientSwitched: " + newMasterClient.NickName);
        MasterClientDooo();
        if (newMasterClient.IsLocal)
        {
            LocalReady(true);
        }
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnLeftRoom()
    {
        // Debug.Log(PhotonNetwork.NickName + " left room with room name " + PhotonNetwork.CurrentRoom.Name);
        base.OnLeftRoom();
        PhotonNetWorkManager.isConnecting = true;
    }

    #endregion


    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // TODO Sync room game information
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(RoomGameInfo);
        }
        else
        {
            // Network player, receive data
            this.RoomGameInfo = stream.ReceiveNext();

        }
    }

    #endregion


    #region RPCs function
    /** RPCs and RaiseEvent:  https://doc.photonengine.com/en-us/pun/current/gameplay/rpcsandraiseevent
     * 
     * Remote Procedure Calls
     * 
     * You can add multiple parameters provided PUN can serialize them (Read about "Serialization in Photon")
     * Serialization in Photon: https://doc.photonengine.com/en-us/pun/current/reference/serialization-in-photon
     * 
     * If you want to send an object array as a parameter of an RPC method, you need to cast it to object type first.
     * 
     * example:  
     *  object[] objectArray = GetDataToSend();
     * photonView.RPC("RpcWithObjectArray", RpcTarget.All, objectArray as object);
     * 
     * [PunRPC]
     * void RpcWithObjectArray(object[] objectArray){ // ... }
     * 
     */


    //发送消息
    public InputField contentInput;//聊天输入框
    public GameObject textPrefab;//文本预制体
    public Transform layoutContent;//父物体

    public void ClearChatMessage()
    {
        for (int i = 0; i < layoutContent.childCount; i++)
        {
            Destroy(layoutContent.GetChild(i).gameObject);
        }
    }

    // 点击聊天框发送按钮，调用该函数
    public void SendMessInfoBtn()
    {
        
        // 获取聊天输入框内容
        // 采取消息和昵称分离模式，便于之后的设计
        string info = contentInput.text;
        if (!string.IsNullOrEmpty(info))
        {
            print("local player sent: " + info);

            // 发送消息给其他人
            photonView.RPC("ChatMessage", RpcTarget.Others, info);

            // diff text color 
            // 生成我方聊天消息 
            GameObject textobj = Instantiate(textPrefab, layoutContent);

            // 这里可以调整
            textobj.GetComponentInChildren<Text>().text = PhotonNetwork.LocalPlayer.NickName + ": " + info;
        }
    }

    [PunRPC]
    void ChatMessage(string mess, PhotonMessageInfo info)
    {
        // 生成其他玩家聊天消息 textobj
        GameObject textobj = Instantiate(textPrefab, layoutContent);

        // 将聊天内容放进 textobj
        textobj.GetComponentInChildren<Text>().text = info.Sender.NickName + ":" + mess;

        // the photonView.RPC() call is the same as without the info parameter.
        // the info.Sender is the player who called the RPC.
        print("received: [" + mess + "]");
        Debug.LogFormat("Info: Sender [{0}]   photonView [{1}]  SentServerTime [{2}]", info.Sender.NickName, info.photonView, info.SentServerTime);
    }



    private IEnumerator MoveToGameScene()
    {
        // Temporary disable processing of futher network messages
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("");//LoadNewScene(newSceneName); // custom method to load the new scene by name
        while (PhotonNetwork.LevelLoadingProgress != 1)
        {
            yield return null;
        }
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    #endregion
}
