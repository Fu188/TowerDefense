using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ooad
{
    public class LobbyController : MonoBehaviour
    {
        private static LobbyController _instance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        private static LobbyService lobbyService;

        //private static ArrayList roomCards;

        public static Dictionary<string, RoomCard> roomPool;

        private static readonly char separator = ':';


        //public delegate void callback();


        private LobbyController()
        {
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            lobbyService = LobbyService.GetLobbyServiceInstance();
            if (lobbyService == null)
            {
                UnityEngine.Debug.LogWarning("GetLobbyServiceInstance return null");
            }

            roomPool = new Dictionary<string, RoomCard>();
        }

        public void Start()
        {

        }

        public static LobbyController GetLobbyControllerInstance()
        {

            return _instance;
        }


        public Dictionary<string, RoomCard> GetRoomPool()
        {
            return roomPool;
        }

        public void RefreshRoomPool()
        {
            if (lobbyService.NeedUpdate(TimeUtil.NowInString()))
            {
                lobbyService.GetRoomPool();

            }
        }

        public void CreateRoom(string roomName, byte maxPlayerNum)
        {
            if (string.IsNullOrEmpty(roomName) || maxPlayerNum < 2 || maxPlayerNum > 15)
            {
                return;
            }
            string regionAndKey = lobbyService.ApplyNewRoom(maxPlayerNum);
            if (string.IsNullOrEmpty(regionAndKey))
            {
                // TODO 申请失败
                UnityEngine.Debug.Log("No licenses are available");
                return;
            }
            UnityEngine.Debug.Log("Get connect info -->  " + regionAndKey);

            RoomController.localRoomCard = new RoomCard(regionAndKey, roomName, maxPlayerNum);
            //Connect(regionAndKey);
        }

        public void JoinRoom(RoomCard roomCard)
        {
            RoomController.localRoomCard = roomCard;
            //Connect(roomCard.GetregionAndKey());
        }

        private void Connect(string regionAndKey)
        {
            string[] info = regionAndKey.Split(separator);

            string region = info[0];

            if (region.Equals("keykeeper"))
            {
                string ip = info[1];
                string port = info[2];
                PhotonNetWorkManager.ConnetToMaster(ip, int.Parse(port));
            }
            else
            {
                string appId = info[1];
                string server = "";
                PhotonNetWorkManager.ConnetToMaster(appId, region, server);
            }
        }

        public void OnConnectedToMaster()
        {
            PhotonNetWorkManager.JoinOrCreateRoom(RoomController.localRoomCard.GetRoomNhame(), RoomController.localRoomCard.GetMaxPlayerNum());
        }

        public static void OnCreateRoomFailed()
        {
            //TODO: CALL LobbyManager.OnCreateRoomFailed();
        }

        public static void OnDisconnected()
        {
            if (RoomController.localRoomCard != null)
            {
                lobbyService.RemoveFromRoomPool(RoomController.localRoomCard);
            }
        }

    }
}