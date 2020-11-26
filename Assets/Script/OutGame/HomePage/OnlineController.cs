using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineController : MonoBehaviour
{

    public List<ConnectInfo> ConnectInfos;
    public GameObject StartMask;
    public GameObject OnlinePanel;
    public GameObject ChooseModePanel;
    public GameObject ChooseServerPanel;


    private static OnlineController _instance { get; set; }

    public static OnlineController GetOnlineControllerInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

    public void GetServers()
    {
        HomePageManager.GetHomePageManagerInstance().PreparationPanelInit(HomePageManager.GetHomePageManagerInstance().OnlinePreparationPanel);
        for (int i = 0; i < ChooseServerPanel.transform.GetChild(1).childCount; i++)
        {
            Destroy(ChooseServerPanel.transform.GetChild(1).GetChild(i).gameObject);
        }
        for (int i = 0; i < ConnectInfos.Count; i++)
        {
            GameObject NewServer = (GameObject)Instantiate(Resources.Load("Prefabs/Server"));
            Button JoinBtn = NewServer.GetComponentInChildren<Button>();
            Text ServerTitle = NewServer.GetComponentInChildren<Text>();
            ServerTitle.text = ConnectInfos[i].GetContent();
            ConnectInfo ci = ConnectInfos[i];
            JoinBtn.onClick.AddListener(delegate ()
            {
                PhotonNetWorkManager.ConnetToMaster(ci);
                HomePageManager.GetHomePageManagerInstance().StartLoading();
            });

            NewServer.transform.SetParent(ChooseServerPanel.transform.GetChild(1));
        }
        ChooseModePanel.SetActive(false);
        ChooseServerPanel.SetActive(true);
    }

    public void OnJoinedLobbyPanelControl()
    {
        StartMask.SetActive(false);
        OnlinePanel.SetActive(true);
        OnlinePanel.transform.GetChild(0).gameObject.SetActive(true);
        OnlinePanel.transform.GetChild(1).gameObject.SetActive(false);
        HomePageManager.GetHomePageManagerInstance().EndLoading();
    }

    public void OnJoinedRoomPanelControl()
    {
        OnlinePanel.transform.GetChild(0).gameObject.SetActive(false);
        OnlinePanel.transform.GetChild(1).gameObject.SetActive(true);
        HomePageManager.GetHomePageManagerInstance().EndLoading();
    }

    public void JoinOrCreate()
    {
        InputField inputField = OnlinePanel.transform.GetChild(0).gameObject.GetComponentInChildren<InputField>();
        if(inputField.text.Length > 15)
        {
            Debug.Log("too long room name");
        }
        else
        {
            HomePageManager.GetHomePageManagerInstance().StartLoading();
            PhotonNetWorkManager.JoinOrCreateRoom(inputField.text, 2);
        }
    }

    public void LeaveLobby()
    {
        PhotonNetWorkManager.LeftLobby();
    }

    public void ModeReturn()
    {
        ChooseModePanel.SetActive(true);
        ChooseServerPanel.SetActive(false);
    }

    public void ChooseDungeon()
    {
        print(PhotonNetwork.LocalPlayer.CustomProperties);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
