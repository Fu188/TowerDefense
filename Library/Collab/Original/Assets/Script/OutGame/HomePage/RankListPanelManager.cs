using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankListPanelManager : MonoBehaviour
{
    private static RankListPanelManager _instance;
    private static User user;

/*    public GameObject TotalRank;
    public GameObject WeekRank;
    public GameObject MonthRank;
    public Button TotalRankBtn;
    public Button WeekRankBtn;
    public Button MonthRankBtn;
    public GameObject[] RankLists;
    public Button[] Btns;*/


    public static List<RankEntry> DRank = new List<RankEntry>();
    public static List<RankEntry> WRank = new List<RankEntry>();
    public static List<RankEntry> MRank = new List<RankEntry>();
    public static RankEntry MyDRank;
    public static RankEntry MyWRank;
    public static RankEntry MyMRank;

    public GameObject DRankContainer;
    public GameObject WRankContainer;
    public GameObject MRankContainer;
    public Transform MyDRankTransform;
    public Transform MyWRankTransform;
    public Transform MyMRankTransform;

    private RankListPanelManager()
    {
    }

    public static RankListPanelManager GetRankListPanelManagerInstance()
    {
        if (_instance == null)
            print("null");
        return _instance;
    }

    void Awake()
    {
        user = User.GetUserInstance();
        if (_instance == null)
            _instance = this;
    }

    public async void RankListInitialize()
    {
        await updateDRank();
        await updateWRank();
        await updateMRank();
        UpdateRankList(DRankContainer, DRank);
        UpdateRankList(WRankContainer, WRank);
        UpdateRankList(MRankContainer, MRank);
        UpdateRankEntryPanel(MyDRankTransform, MyDRank);
        UpdateRankEntryPanel(MyWRankTransform, MyWRank);
        UpdateRankEntryPanel(MyMRankTransform, MyMRank);
    }

    public void UpdateRankList(GameObject RankContainer, List<RankEntry> rankEntries)
    {
        int length = rankEntries.Count < 10 ? rankEntries.Count : 10;
        for (int i = 0; i < length; i++)
        {
            UpdateRankEntryPanel(RankContainer.transform.GetChild(i), rankEntries[i]);
        }
        for (int i = length; i < 10; i++)
        {
            RankContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdateRankEntryPanel(Transform RankEntryPanel, RankEntry rankEntry)
    {
        ShowImage(RankEntryPanel.GetChild(1).GetChild(0).GetComponent<Image>(), rankEntry.Avatar);
        RankEntryPanel.GetChild(2).GetComponent<Text>().text = rankEntry.NickName;
        RankEntryPanel.GetChild(3).GetComponent<Text>().text = "Lv. " + rankEntry.Level;
        RankEntryPanel.GetChild(5).GetComponent<Text>().text = "Point: " + rankEntry.Value.ToString("N0");
        if(rankEntry.Rank > 3)
        {
            RankEntryPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Textures/Rank", typeof(Sprite)) as Sprite;
            RankEntryPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = rankEntry.Rank.ToString();
        }
        else
        {
            RankEntryPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Textures/Rank" + rankEntry.Rank, typeof(Sprite)) as Sprite;
        }
    }

    #region showUrlImage
    public void ShowImage(Image image, string Url)
    {
        print("RankListPanelManager: image id = "+ image.GetInstanceID()+ " ShowImage url = " + Url);
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
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
    #endregion

   /* void Start()
    {
        RankLists = new GameObject[] { TotalRank, WeekRank, MonthRank };
        Btns = new Button[] { TotalRankBtn, WeekRankBtn, MonthRankBtn };

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setTypeActive(int index)
    {
        for (int i = 0; i < RankLists.Length; i++)
        {
            if (i == index)
            {
                RankLists[i].SetActive(true);
                Btns[i].interactable = false;
            }
            else
            {
                RankLists[i].SetActive(false);
                Btns[i].interactable = true;
            }
        }
    }*/





    async Task<bool> updateDRank()
    {
        DRank = await RankDataService.GetHairRank(1, 10, "D");
        List<RankEntry> tmp = await RankDataService.GetHairRankAroudMe("D");
        if (tmp.Count != 0)
        {
            MyDRank = tmp[0];
        }
        else
        {
            MyDRank = GetDefaultRnakEntry();
        }
        return true;
    }

    async Task<bool> updateWRank()
    {
        WRank = await RankDataService.GetHairRank(1, 10, "W");
        List<RankEntry> tmp = await RankDataService.GetHairRankAroudMe("W");
        if (tmp.Count != 0)
        {
            MyWRank = tmp[0];
        }
        else
        {
            MyWRank = GetDefaultRnakEntry();
        }
        return true;
    }

    async Task<bool> updateMRank()
    {
        MRank = await RankDataService.GetHairRank(1, 10, "M");
        List<RankEntry> tmp = await RankDataService.GetHairRankAroudMe("M");
        if (tmp.Count != 0)
        {
            MyMRank = tmp[0];
        }
        else
        {
            MyMRank = GetDefaultRnakEntry();
        }
        return true;
    }

    private RankEntry GetDefaultRnakEntry()
    {
        return new RankEntry
        {
            Avatar = user.GetAvatar(),
            Level = user.GetLevel(),
            NickName = user.GetNickname(),
            Rank = -1,
            Value = 0
        };
    }
}
