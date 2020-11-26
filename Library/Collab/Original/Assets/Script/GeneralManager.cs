using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class GeneralManager : MonoBehaviour
{
    [HideInInspector]
    public static GeneralManager Instance { get; private set; }

    public User user;
    void Awake()
    {
        Instance = this;
        user = User.GetUserInstance();
        GenerateAchievement(user.GetAchievement());
        GenerateCard(user.GetCard());
        //GenerateCard("");
        //GenerateProp(user.GetProp());
        GenerateProp("");
        GenerateKkMails(1);
    }

    

    public List<Card> cards = new List<Card>();
    public ArrayList enemies = new ArrayList();
    public List<Achievement> achievements = new List<Achievement>();
    public List<Prop> props = new List<Prop>();
    public ArrayList buffs = new ArrayList();
    public List<KkMail> mails = new List<KkMail>();
    public List<RankEntry> DRank = new List<RankEntry>();
    public List<RankEntry> WRank = new List<RankEntry>();
    public List<RankEntry> MRank = new List<RankEntry>();
    public RankEntry MyDRank;
    public RankEntry MyWRank;
    public RankEntry MyMRank;

    void GenerateCard(string haveCards)
    {
        string newCards = Resources.Load("Content/Cards").ToString();
        JArray jaHave = (JArray)JsonConvert.DeserializeObject(haveCards);
        JArray jaNew = (JArray)JsonConvert.DeserializeObject(newCards);
        int jaHaveCount = 0;
        if (jaHave != null)
        {
            jaHaveCount = jaHave.Count;
            foreach (JToken j in jaHave)
            {
                Card card = JsonConvert.DeserializeObject<Card>(j.ToString());
                cards.Add(card);
            }
        }
        if (jaNew.Count > jaHaveCount)
        {
            for (int i = jaHaveCount; i < jaNew.Count; i++)
            {
                Card card = JsonConvert.DeserializeObject<Card>(jaNew[i].ToString());
                cards.Add(card);
            }
        }
    }

    void GenerateProp(string haveProps)
    {
        string newProps = Resources.Load("Content/Props").ToString();
        JArray jaHave = (JArray)JsonConvert.DeserializeObject(haveProps);
        JArray jaNew = (JArray)JsonConvert.DeserializeObject(newProps);
        int jaHaveCount = 0;
        if (jaHave != null)
        {
            jaHaveCount = jaHave.Count;
            foreach (JToken j in jaHave)
            {
                Prop prop = JsonConvert.DeserializeObject<Prop>(j.ToString());
                props.Add(prop);
            }
        }
        if (jaNew.Count > jaHaveCount)
        {
            for (int i = jaHaveCount; i < jaNew.Count; i++)
            {
                Prop prop = JsonConvert.DeserializeObject<Prop>(jaNew[i].ToString());
                props.Add(prop);
            }
        }
    }

    void GenerateAchievement(string haveAchievements)
    {
        /*string fileName = "Assets/Script/Content/Achievements.json";
        string jsonArrayText = File.ReadAllText(fileName);*/
        string newAchievements = Resources.Load("Content/Achievements").ToString();
        JArray jaHave = (JArray)JsonConvert.DeserializeObject(haveAchievements);
        JArray jaNew = (JArray)JsonConvert.DeserializeObject(newAchievements);
        int jaHaveCount = 0;
        if (jaHave != null)
        {
            jaHaveCount = jaHave.Count;
            foreach (JToken j in jaHave)
            {
                Achievement achievement = JsonConvert.DeserializeObject<Achievement>(j.ToString());
                achievements.Add(achievement);
            }
        }
        if(jaNew.Count > jaHaveCount)
        {
            for(int i = jaHaveCount; i < jaNew.Count; i++)
            {
                Achievement achievement = JsonConvert.DeserializeObject<Achievement>(jaNew[i].ToString());
                achievements.Add(achievement);
            }
        }
    }

    void GenerateKkMails(long recieverId)
    {
        this.mails = KkMailService.GetKkMailServiceInstance().GetReceivedKkMails(recieverId);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
