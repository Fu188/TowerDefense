using System;
using UnityEngine;

/// <summary>
/// 用户类，使用单例模式，通过 User.GetUserInstance() 方法获取
/// </summary>
public class User
{
    #region Private attribute
    private static User _instance;
    // 定义一个标识确保线程同步
    private static readonly object locker = new object();

    [SerializeField]
    private long userId;
    [SerializeField]
    private string email;
    [SerializeField]
    private string pwd;
    [SerializeField]
    private string nickname;
    [SerializeField]
    private string character;
    [SerializeField]
    private string avatar;
    [SerializeField]
    private int exp;
    [SerializeField]
    private int hair;
    [SerializeField]
    private int nikeCoin;
    [SerializeField]
    private int credit;
    [SerializeField]
    private string prop;
    [SerializeField]
    private string card;
    [SerializeField]
    private string manual;
    [SerializeField]
    private string achievement;
    [SerializeField]
    private string createTime;
    [SerializeField]
    private string lastActiveTime;

    #endregion

    #region Private function
    private User()
    {
    }
    #endregion

    public static User GetUserInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new User();
                }
            }
        }
        return _instance;
    }


/*    /// <summary>
    /// 更新头发数量，默认一分钟恢复一根头发
    /// </summary>
    /// <param name="hairChange">头发改变数 默认为0，即刷新被动更新的头发数量</param>
    /// <param name="newHairPerMin">每分钟增长头发数</param>
    public void UpdateHairAndTime(int hairChange = 0, double newHairPerMin = 0.2)
    {
        DateTime dNow = TimeUtil.Now();
        double diffMinutes = TimeUtil.CalculateDiff(dNow, lastActiveTime);
        int newHairNum = hair + (int)(diffMinutes* newHairPerMin) + hairChange;
        int hairNumLimit = 79 + GetLevel();
        newHairNum = newHairNum > hairNumLimit ? hairNumLimit : newHairNum;
        hair = newHairNum;
        lastActiveTime = TimeUtil.DateTimeToString(dNow);
    }*/

    public int GetLevel()
    {
        // Avatar.sprite
        int[] levels = Assets.Scripts.Constants.Experience;
        int i = 0;
        while (i < levels.Length && levels[i] < exp)
        {
            i++;
        }
        ++i;
        return i;
    }


    #region Getter and Setter function

    /*public User(long userId)
    {
        this.userId = userId;
    }

    public User(string email)
    {
        this.email = email;
    }

    public User(string email, string pwd)
    {
        this.email = email;
        this.pwd = pwd;
    }

    public User(string email, string pwd, string nickname)
    {
        this.email = email;
        this.pwd = pwd;
        this.nickname = nickname;
    }*/

    public long GetUserId()
    {
        return userId;
    }

    /*public User SetUserId(long userId)
    {
        this.userId = userId;
        return this;
    }*/

    public string GetEmail()
    {
        return email;
    }

    /*public User SetEmail(string email)
    {
        this.email = email;
        return this;
    }*/

    public string GetPwd()
    {
        return pwd;
    }

    /*public User SetPwd(string pwd)
    {
        this.pwd = pwd;
        return this;
    }*/

    public string GetNickname()
    {
        return nickname;
    }

    public User SetNickname(string nickname)
    {
        this.nickname = nickname;
        return this;
    }

    public string GetCharacter()
    {
        return character;
    }

    public User SetCharacter(string character)
    {
        this.character = character;
        return this;
    }

    public string GetAvatar()
    {
        return avatar;
    }

    public User SetAvatar(string avatar)
    {
        this.avatar = avatar;
        return this;
    }

    public int GetExp()
    {
        return exp;
    }

    public User SetExp(int exp)
    {
        this.exp = exp;
        return this;
    }

    public int GetHair()
    {
        return hair;
    }

    public User SetHair(int hair)
    {
        this.hair = hair;
        return this;
    }

    public int GetNikeCoin()
    {
        return nikeCoin;
    }

    public User SetNikeCoin(int nikeCoin)
    {
        this.nikeCoin = nikeCoin;
        return this;
    }

    public int GetCredit()
    {
        return credit;
    }

    public User SetCredit(int credit)
    {
        this.credit = credit;
        return this;
    }

    public string GetProp()
    {
        return prop;
    }

    public User SetProp(string prop)
    {
        this.prop = prop;
        return this;
    }

    public string GetCard()
    {
        return card;
    }

    public User SetCard(string card)
    {
        this.card = card;
        return this;
    }

    public string GetManual()
    {
        return manual;
    }

    public User SetManual(string manual)
    {
        this.manual = manual;
        return this;
    }

    public string GetCreateTime()
    {
        return createTime;
    }

    /*public User SetCreateTime(string createTime)
    {
        this.createTime = createTime;
        return this;
    }*/

    public string GetAchievement()
    {
        return achievement;
    }

    public User SetAchievement(string achievement)
    {
        this.achievement = achievement;
        return this;
    }

    public string GetLastActiveTime()
    {
        return lastActiveTime;
    }

    public User SetLastActiveTime(string lastActiveTime)
    {
        this.lastActiveTime = lastActiveTime;
        return this;
    }
#endregion

   

    public override string ToString()
    {
        return "User{" +
                "userId=" + userId +
                ", email='" + email + '\'' +
                ", pwd='" + pwd + '\'' +
                ", nickname='" + nickname + '\'' +
                ", avatar=" + avatar +
                ", exp=" + exp +
                ", hair=" + hair +
                ", nikeCoin=" + nikeCoin +
                ", credit=" + credit +
                ", prop=" + prop +
                ", card=" + card +
                ", manual=" + manual +
                ", achievement='" + achievement + '\'' +
                ", createTime=" + createTime +
                ", lastActiveTime=" + lastActiveTime +
                '}';
    }
}