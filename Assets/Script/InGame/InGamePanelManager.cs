using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

public class InGamePanelManager : MonoBehaviour
{
    public GameObject Pause_Panel;
    public GameObject Won_Panel;
    public GameObject Lost_Panel;
    public int SpeedStatus;

    public int ChooseMode; //0: No choose; 1: Choose card; 2: Choose prop.

    public int[] Cards;
    public GameObject[] CardBoxs;
    public int[] ChosenCards;
    public int CardIndex;

    public int[] Props;
    public GameObject[] PropBoxs;
    public int[] ChosenProps;
    public int PropIndex;

    public GameObject SelectBannerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.sceneCount > 1)
            SceneManager.UnloadSceneAsync("Loading");
        Pause_Panel = GameObject.Find("Canvas/Base_Panel/Pause_Mask");
        Won_Panel = GameObject.Find("Canvas/Base_Panel/Won_Mask");
        Lost_Panel = GameObject.Find("Canvas/Base_Panel/Lost_Mask");
        Time.timeScale = 1;
        SpeedStatus = 0;
        Transform CardContainer = GameObject.Find("Canvas/Base_Panel/Cards").transform;
        CardBoxs = new GameObject[8];
        for (int i = 0; i < CardBoxs.Length; i++)
        {
            CardBoxs[i] = CardContainer.GetChild(i).gameObject;
        }
        CardIndex = 0;
        ChosenCards = HomePageManager.GetHomePageManagerInstance().isChosenCards;
        List<Card> cards = GeneralManager.Instance.cards;
        for(int i = 0; i < 8; i++)
        {
            if(ChosenCards[i] > -1)
            {
                CardBoxs[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load(cards[ChosenCards[i]].ImgPath_small, typeof(Sprite)) as Sprite;
            }
        }

        Transform PropContainer = GameObject.Find("Canvas/Base_Panel/Props").transform;
        PropBoxs = new GameObject[8];
        for (int i = 0; i < PropBoxs.Length; i++)
        {
            PropBoxs[i] = PropContainer.GetChild(i).gameObject;
        }
        PropIndex = 0;
        ChosenProps = HomePageManager.GetHomePageManagerInstance().isChosenProps;
        List<Prop> props = GeneralManager.Instance.props;
        for (int i = 0; i < 8; i++)
        {
            if (ChosenProps[i] > -1)
            {
                PropBoxs[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load(props[ChosenProps[i]].ImgPath, typeof(Sprite)) as Sprite;
            }
        }

    }

    public void Pause(){
        Pause_Panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Back(){
        Pause_Panel.SetActive(false);
        Time.timeScale = 1 + SpeedStatus * 0.5f;
    }

    public void Win()
    {
        Won_Panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Lose()
    {
        Lost_Panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Exit(){
        GameNetworkManager.playing = false;
        Scene MainScene = SceneManager.GetSceneByName("Main");
        GameObject[] list = MainScene.GetRootGameObjects();
        GameObject camera = list[0];
        foreach (GameObject b in list)
        {   
            if(b.name == "Canvas"){
                b.SetActive(true);
                RawImage rawImage = b.transform.GetChild(3).GetComponent<RawImage>();
                rawImage.color = Color.clear;
                rawImage.enabled = false;
            }
            if(b.name == "face_prefab")
            {
                b.SetActive(true);
            }
        }
            
       
        camera.GetComponent<AudioListener>().enabled = true;
        SceneManager.UnloadSceneAsync("InGame");
    }

    public void SpeedSwitch(){
        SpeedStatus = (SpeedStatus+1) % 3;
        Time.timeScale = 1 + SpeedStatus * 0.5f;
        string s = "";
        switch(SpeedStatus){
            case 0:
                s = "×1.0";
                break;
            case 1:
                s = "×1.5";
                break;
            case 2:
                s = "×2.0";
                break;
        }
        GameObject.Find("Canvas/Base_Panel/Speed_Bg/Speed_Value").GetComponent<Text>().text = s;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.Won:
                Win();
                break;
            case GameState.Lost:
                Lose();
                break;
        }
        CardIndex %= 8;
        PropIndex %= 8;
        if (Input.GetKeyDown(KeyboardManager.Instance.SwitchMode))
        {
            if (ChooseMode == 1)
            {
                CardBoxs[CardIndex].transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (ChooseMode == 2)
            {
                PropBoxs[PropIndex].transform.GetChild(1).gameObject.SetActive(false);
            }
            ChooseMode += 1;
        }
        ChooseMode %= 3;
        switch (ChooseMode)
        {
            case 0:
                break;
            case 1:
                if (!CardBoxs[CardIndex].transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    CardBoxs[CardIndex].transform.GetChild(1).gameObject.SetActive(true);
                }
                if (Input.GetKeyDown(KeyboardManager.Instance.SwitchItem))
                {
                    CardBoxs[CardIndex].transform.GetChild(1).gameObject.SetActive(false);
                    CardIndex += 1;
                }
                else if (Input.GetKeyDown(KeyboardManager.Instance.CardModePut))
                {
                    if (ChosenCards[CardIndex] > -1)
                        GameManager.Instance.createPeople(ChosenCards[CardIndex]);
                }
                else if (Input.GetKeyDown(KeyboardManager.Instance.CardModeUpgrade))
                {
                    GameManager.Instance.updatePeople();
                }
                else if (Input.GetKeyDown(KeyboardManager.Instance.CardModeRemove))
                {
                    GameManager.Instance.removePeople();
                }

                break;
            case 2:
                if (!PropBoxs[PropIndex].transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    PropBoxs[PropIndex].transform.GetChild(1).gameObject.SetActive(true);
                }
                if (Input.GetKeyDown(KeyboardManager.Instance.SwitchItem))
                {
                    PropBoxs[PropIndex].transform.GetChild(1).gameObject.SetActive(false);
                    PropIndex += 1;
                }
                else if (Input.GetKeyDown(KeyboardManager.Instance.PropModeUse))
                {
                    if (ChosenProps[PropIndex] > -1)
                        GameManager.Instance.useProp(ChosenProps[PropIndex]);
                }

                break;
        }

    }
}
