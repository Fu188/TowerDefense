using System;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    static User user;
    static int[] levels = Assets.Scripts.Constants.Experience;

    CountDownTimer hairTimer;

    // cache user info
    public static int CurrentLevel, CurrentHairNum, CurrentExp, CurrentNikeCoin, CurrentCredit;
    public static string hairTimeText;

    // constant
    const double NewHairPerMin = 0.2;
    const int GrowOneHairCostMinute = 5;
    const int MaxLevel = 45;

    // Variable, which may be updated after the user data is updated
    public static int HairNumLimit;

    // Cache calculated user information
    public static int LevelExp, RequireExp;


    // Start is called before the first frame update
    void Awake()
    {
        user = User.GetUserInstance();
        // 更新 user 头发以及 lastActiveTime

        // 先获取等级，因为等级影响头发数量的计算、更新
        CurrentLevel = user.GetLevel();
        // 刷新头发上限
        HairNumLimit = 79 + CurrentLevel;

        print("UserInfoManager: " + user);
        UpdateUserHairAndTime();
        UpdateUserInfoCache();
        print("UserInfoManager: " +user);
        
    }

    private void Start()
    {
        hairTimer = new CountDownTimer(GrowOneHairCostMinute * 60);
    }

    // Update is called once per frame
    private void Update()
    {
        GrowHair();
    }


    /// <summary>
    /// 生长头发（每 GrowOneHairCostMinute 分钟调用一次）
    /// </summary>
    public void GrowHair()
    {
        if (CurrentHairNum >= HairNumLimit)
        {
            hairTimer.End();
            hairTimeText = "-:--";
        }
        else if (hairTimer.IsStoped)
        {
            hairTimer.Reset(5);
            ++CurrentHairNum;
        }
        else
        {
            int timerMin = (int)hairTimer.CurrentTime / 60;
            int timerSec = (int)hairTimer.CurrentTime % 60;
            hairTimeText = timerMin + ":" + (timerSec < 10 ? ("0" + timerSec) : (timerSec.ToString()));
        }
    }


    public static void UpdateUserInfoCache()
    {
        // 先获取等级，因为等级影响头发数量的计算、更新
        CurrentLevel = user.GetLevel();
        // 刷新头发上限
        HairNumLimit = 79 + CurrentLevel;

        // 刷新 本地 hair cache
        CurrentHairNum = user.GetHair();

        // 获取经验值，计算 LevelExp、RequireExp
        CurrentExp = user.GetExp();
        if (CurrentLevel == 1)
        {
            LevelExp = CurrentExp;
            RequireExp = levels[0];
        }
        else
        {
            LevelExp = CurrentExp - levels[CurrentLevel - 2];
            RequireExp = levels[CurrentLevel - 1] - levels[CurrentLevel - 2];
        }
    }


    /// <summary>
    /// 更新头发数量，默认五分钟恢复一根头发
    /// </summary>
    /// <param name="hairChange">头发改变数 默认为0，即刷新被动更新的头发数量</param>
    /// <param name="newHairPerMin">每分钟增长头发数</param>
    public static void UpdateUserHairAndTime(int hairChange = 0, double newHairPerMin = NewHairPerMin)
    {
        DateTime dNow = TimeUtil.Now();
        double diffMinutes = TimeUtil.CalculateDiff(dNow, user.GetLastActiveTime());
        int newHairNum = user.GetHair() + (int)(diffMinutes * newHairPerMin) + hairChange;
        print("user.GetHair()=" + user.GetHair() + "   diffMinutes=" + diffMinutes + "  newHairPerMin=" + newHairPerMin + "  (int)(diffMinutes * newHairPerMin)="+ (int)(diffMinutes * newHairPerMin)+
             "  hairChange=" + hairChange);
        newHairNum = newHairNum >= HairNumLimit ? HairNumLimit : newHairNum;
        user.SetHair(newHairNum);
        print(string.Format("dNow={0}, LastActiveTime={1}, diffMinutes={2}, newHairNum={3}", dNow, user.GetLastActiveTime(),diffMinutes, newHairNum));
        user.SetLastActiveTime(TimeUtil.DateTimeToString(dNow));
    }



    /// <summary>
    /// 掉落头发（开始游戏时消耗头发）
    /// </summary>
    /// <param name="lost"></param>
    public static void LoseHair(int lost)
    {
        UpdateUserHairAndTime(lost);
        UpdateUserInfoCache();
    }


    /// <summary>
    /// 经验值更新, user 、cache 
    /// </summary>
    /// <param name="change"></param>
    public static void UpdateExp(int change)
    {
        CurrentExp += change;
        user.SetExp(CurrentExp);
        UpdateUserInfoCache();
    }


    /// <summary>
    /// 你科币更新, user 、cache 
    /// </summary>
    /// <param name="change"></param>
    public static void UpdateNikeCoin(int change)
    {
        CurrentNikeCoin += change;
        user.SetNikeCoin(CurrentNikeCoin);
        UpdateUserInfoCache();
    }

    /// <summary>
    /// 学分更新, user 、cache 
    /// </summary>
    /// <param name="change"></param>
    public static void UpdateCredit(int change)
    {
        CurrentCredit += change;
        user.SetCredit(CurrentCredit);
        UpdateUserInfoCache();
    }


}
