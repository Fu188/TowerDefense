using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts{
    public static class Constants
    {
        public static readonly int InitialEnemyHealth = 50;
        public static readonly int CoinAward = 40;
        public static readonly float CoinSpawnSplitTime = 0.2f;
        public static readonly int CoinSpawnLimitTime = 15;
        public static readonly float MinDistanceForPeopleToShoot = 5f;
        public static readonly float MinDistanceToDestroyPeople = 8f;
        public static readonly float CoinStickTime = 10f;
        public static readonly float MinDistanceForCoinToStick = 30f;
        public static readonly int[] Experience = {
            100, 230, 399, 619, 905, 1276, 1759, 2386, 3202, 4262,
            9262, 15262, 22462, 31102, 41470, 53912, 68842, 86758,
            108257, 134056, 254056, 398056, 570856, 778216, 1027048,
            1325646, 1683964, 2113946, 2629924, 3249098, 7249098,
            12049098, 17809098, 24721098, 33015498, 42968778, 54912714,
            69245437, 86444705, 107083826, 128083826,150133826,173286326,
            197596451};
    }

    public class Card
    {
        public int Id;
        public string Name;
        public bool IsObtained;
        public int Price;
        public string ImgPath_large;
        public string ImgPath_small;
        public string Description;
        public float ShootWaitTime;
        public int CreateCost;
        public int Attack;
    }

    public class EnemyInfo
    {
        public int Id;
        public string Name;
        public bool isObtained;
        public string ImgPath_160_468;
        public float Hp;
        public float Speed;
        public string PrefabPath;
        public string Description;
    }

    public class Achievement
    {
        public int Id;
        public string Name;
        public string ImgPath;
        public string Description;
        public AchievementType AchievementType;
        public int Now;
        public int Target;
        public bool IsRewarded;
        public RewardType[] RewardType;
        public int[] RewardNum;
    }

    public class Prop
    {
        public int Id;
        public string Name;
        public bool IsObtained;
        public string ImgPath;
        public float CdTime; //cd
        public float Duration;
        public int InGameNumLimit;
        public string Description;
        //public ArrayList BuffList;
        public ArrayList BuffTypeList;
        public ArrayList BuffRatioList;
        public ArrayList BuffOffsetList;
    }

    public class Buff
    {
        public int Id;
        public string Name;
        public string ImgPath;
        public BuffType BuffType;
        public float Ratio;
        public float Offset;
        public float Duration;
        public string Description;
    }

    public enum AchievementType
    {
        Level, NKBSum, NKBConsume, CreditSum, CreditConsume
    }

    public enum RewardType
    {
        Exp, NKB, Credit
    }

    public enum BuffType
    {
        PlayerSpeed, CardATK, EnergyGet, EnermySpeed, EnermyHP, BaseHP
    }
}
