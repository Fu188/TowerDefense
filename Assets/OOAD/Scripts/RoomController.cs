using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ooad
{
    public class RoomController : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static RoomController _instance;//单例

        private static LobbyService lobbyService;

        public static RoomCard localRoomCard;
        /*public GameObject readyBtn;//准备按钮
        public GameObject startBtn;//开始游戏按钮*/

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

        public static RoomController GetRoomControllerInstance()
        {
            return _instance;
        }

        private void Start()
        {
            lobbyService = LobbyService.GetLobbyServiceInstance();
        }


        public void LeftRoom()
        {
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                lobbyService.RemoveFromRoomPool(localRoomCard);
            }
            localRoomCard = null;
            PhotonNetwork.Disconnect();
        }


        private void UpdateRoomCard()
        {
            
            /*if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if(localRoomCard == null)
                {
                    Debug.LogWarning("In room but local room card is null");
                    return;
                }
                localRoomCard.SetCurrentPlayerNum(PhotonNetwork.CurrentRoom.PlayerCount);
                lobbyService.UpdateRoomPool(localRoomCard);
            }*/
        }



        #region Photon Callback Funtion

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log(PhotonNetwork.NickName + " joined room with room name "+PhotonNetwork.CurrentRoom.Name+". Number of players in the current room: "
            + PhotonNetwork.CurrentRoom.PlayerCount);

            /*localRoomCard.SetCurrentPlayerNum(PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)//判断是否是房主，是就显示开始游戏按钮，不是就显示准备按钮
            {
                if(localRoomCard == null)
                {
                    Debug.Log("why my room card is null");
                }
                else
                {

                }
                Debug.Log("I am master client, try to update room pool, my room card: " + RoomController.localRoomCard.ToString());
                lobbyService.UpdateRoomPool(localRoomCard);
                *//*readyBtn.SetActive(false);
                startBtn.SetActive(true);*//*
                PhotonNetwork.LoadLevel("DemoDay 1");
            }
            else
            {
                *//*readyBtn.SetActive(true);
                startBtn.SetActive(false);*//*
            }*/
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            UpdateRoomCard();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            UpdateRoomCard();
        }


        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            UpdateRoomCard();
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
        }

        public override void OnLeftRoom()
        {
           // Debug.Log(PhotonNetwork.NickName + " left room with room name " + PhotonNetwork.CurrentRoom.Name);
            base.OnLeftRoom();
            
        }

        #endregion


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
}