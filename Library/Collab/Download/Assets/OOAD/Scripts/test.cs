using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LeanCloud;
using ooad;
using Photon.Pun;
using Photon.Realtime;
using SimpleFileBrowser;
using UnityEngine;

namespace ooad
{
    public class test : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        async void Start()
        {
            //ConnectToChina();

            //Invoke(nameof(test_start), 2);
            //TestEncrypt();
            //PhotonNetwork.Disconnect();
            //TestRoomList();

            //FileTest();
            //ImageUtil.LoadLocalImage();

            //Testleancloud();
            //testParams();

            /*RankEntry r = new RankEntry();
            r.NickName = "test";
            r.Rank = 1;
           
            print(Newtonsoft.Json.JsonConvert.SerializeObject(r));*/
            /* await LeanCloudUtil.LogInAwait("test02@keykeeper.ga", "keykeeperT2pwd");
             *//*await LeanCloudUtil.ChangeAvatarAwait("https://keykeeper.ga:8080/static/img/user/avatar/default.jpg");
             await LeanCloudUtil.ChangeLevelAwait(1);*//*
             await RankDataService.UpdateHairRankData(5,"D");
             List<RankEntry> re =  await RankDataService.GetHairRank(1, 2);
             foreach(RankEntry r in re)
             {
                 print(r.ToString());
             }*/
            //;
        }

         void Testleancloud()
        {

            

            //LeanCloudUtil.GetRankData("5fb2547bbd3b963a9278966f");

        }

        public async void testParams(params string[] s)
        {
            print(s);
            print(s == null);
            List<string> list = new List<string>(s);
            print(list);
            print(list == null);
            var statistics = await AVLeaderboard.GetStatistics(AVUser.CurrentUser);
            foreach(var st in statistics)
            {
                print(st);
            }
        }
        public void FileTest()
        {
            print("begin FileTest()");
            //print(UserController.GetUserControllerInstance().ChangeAvatar(1, "D:/ooad-together.png"));
            print(LeanCloudUtil.UploadFileAsyncWithProgress(1, "user-avatar-1.png", File.ReadAllBytes("D:/ooad-together.png")));
            print("end FileTest()");
        }

        public override void OnConnectedToMaster()
        {

            Debug.Log(PhotonNetwork.NickName + " connected to master server.");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            
            PhotonNetwork.CreateRoom("hello");
        }
        bool ConnectToChina()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cn";
            PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "5035ebaf-9638-46ab-b232-cf74e4d570bc";
            PhotonNetwork.PhotonServerSettings.AppSettings.Server = "ns.photonengine.cn";
            return PhotonNetwork.ConnectUsingSettings();
        }

        public void TestRoomList()
        {
            LobbyService lobbyService = LobbyService.GetLobbyServiceInstance();
            List<ConnectInfo> cis = lobbyService.GetConnectInfos();
            foreach (ConnectInfo ci in cis)
            {
                Debug.Log(ci.ToString());
            }
            PhotonNetWorkManager.ConnetToMaster(cis[0]);

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log(cause);
            Debug.Log(cause.ToString());
        }
        public void TestEncrypt()
        {
            string myPwd = "123456";

            string encodePwd = AESUtil.AESEncrypt(myPwd);
            string decodePwd = AESUtil.AESDecrypt("uZyPPozWHIDvSIA23J4CWg==");

            Debug.Log("encode pwd: " + encodePwd);
            Debug.Log("decode pwd: " + decodePwd);

            string md5Encode = AESUtil.GetMD5(myPwd);
            Debug.Log("md5 encode: " + md5Encode);
        }
        public void TestStart()
        {
            LobbyService lobbyService = LobbyService.GetLobbyServiceInstance();
            ooad.LobbyController lobbyController = ooad.LobbyController.GetLobbyControllerInstance();
            lobbyController.CreateRoom("myRoom", 2);

            Debug.Log("localRoomCard " + RoomController.localRoomCard.ToString());

            Invoke(nameof(LeftRoomTest), 60);
        }

        public void LeftRoomTest()
        {
            Debug.Log("Invoke LeftRoomTest()");
            RoomController roomManager = RoomController.GetRoomControllerInstance();
            roomManager.LeftRoom();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}