using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ooad
{
    public class LobbyService
    {

        private static LobbyService _instance;

        private static readonly object locker = new object();

        private string baseUrl = NetWorkInfo.backEndBaseUrl + "/v1/api";


        private LobbyService()
        {
        }


        public static LobbyService GetLobbyServiceInstance()
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new LobbyService();
                    }
                }
            }
            return _instance;
        }

        public List<ConnectInfo> GetConnectInfos()
        {
            string result = HttpUtil.Get(string.Format("{0}/getConnectInfos", baseUrl));
            Debug.Log(result);
            List<ConnectInfo> connectInfos = JsonConvert.DeserializeObject<List<ConnectInfo>>(result);
            
            string tempKey;
            
            foreach (ConnectInfo ci in connectInfos)
            {
                Debug.Log(ci.ToString());
                tempKey = AESUtil.AESDecrypt(ci.GetKey());
                ci.SetKey(tempKey);
            }
            return connectInfos;
        }

        public string ApplyNewRoom(byte maxPlayerNum)
        {
            string result = HttpUtil.Get(string.Format("{0}/room/applyNewRoom/{1}", baseUrl, maxPlayerNum));
            return AESUtil.AESDecrypt(result);
        }

        public bool UpdateRoomPool(RoomCard roomCard)
        {

            Debug.Log("Try to update room pool, my room card:" + roomCard.ToString());
            return HttpUtil.Put(JsonUtility.ToJson(roomCard), string.Format("{0}/room/updateRoomPool", baseUrl)).Equals("true");
        }

        public bool RemoveFromRoomPool(RoomCard roomCard)
        {
            return HttpUtil.Delete(JsonUtility.ToJson(roomCard), string.Format("{0}/room/removeFromRoomPool", baseUrl)).Equals("true");
        }

        public bool GetRoomPool()
        {
            string result = HttpUtil.Get(string.Format("{0}/room/getRoomPool", baseUrl));
            JsonUtility.FromJsonOverwrite(result, LobbyController.roomPool);
            return true;
        }

        public bool NeedUpdate(string myDate)
        {
            return HttpUtil.Get(string.Format("{0}/room/needUpdate/{1}", baseUrl, myDate)).Equals("true");
        }
    }
}