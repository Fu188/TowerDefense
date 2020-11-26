using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using LeanCloud;
using static LeanCloud.AVClient;

public class RankDataService
{

    private static RankEntry AVRankingToRankEntry(AVRanking avRanking)
    {
        if (avRanking == null)
        {
            return null;
        }
        AVUser player = avRanking.User;
        return new RankEntry
        {
            Rank = avRanking.Rank + 1,
            NickName = player.Get<string>("nickName"),
            Value = avRanking.Value,
            Avatar = player.Get<string>("avatar"),
            Level = player.Get<int>("level")
        };
    }

    private static List<RankEntry> AVRankingToRankEntry(List<AVRanking> avRankings)
    {
        List<RankEntry> rankEntries = new List<RankEntry>();
        if (avRankings != null && avRankings.Count != 0)
        {
            foreach (var avRanking in avRankings)
            {
                AVUser player = avRanking.User;
                rankEntries.Add(new RankEntry
                {
                    Rank = avRanking.Rank + 1,
                    NickName = player.Get<string>("nickName"),
                    Value = avRanking.Value,
                    Avatar = player.Get<string>("avatar"),
                    Level = player.Get<int>("level")
                });
            }
        }  
        return rankEntries;
    }

    private static Dictionary<string, double> AVStatisticToDictionary(List<AVStatistic> avStatistics)
    {
        Dictionary<string, double> dict = new Dictionary<string, double>();
        if(avStatistics!= null && avStatistics.Count != 0)
        {
            foreach (AVStatistic statistic in avStatistics)
            {
                dict.Add(statistic.Name, statistic.Value);
            }
        }
        
        return dict;
    }


    /// <summary>
    /// 更新 Hair 排行榜 日榜、周榜、月榜 中的成绩
    /// </summary>
    /// <param name="hairChange">新消耗的头发数量</param>
    public static async Task<Dictionary<string, double>> UpdateHairRankData(int hairChange)
    {
        List<AVStatistic> avStatistics = await LeanCloudUtil.UpdateDataInLeaderboardAwait(
            new List<string>() { "HairRankD", "HairRankW", "HairRankM" }, new List<double>() { hairChange, hairChange, hairChange });
        return AVStatisticToDictionary(avStatistics);
    }

    /// <summary>
    /// 更新 指定 Hair 排行榜  默认为日榜
    /// </summary>
    /// <param name="hairChange">新消耗的头发数量</param>
    /// <param name="interval">D, W, M  D->日榜 W->周榜 M->月榜</param>
    public static async Task<Dictionary<string, double>> UpdateHairRankData(int hairChange, string interval)
    {
        List<AVStatistic> avStatistics = await LeanCloudUtil.UpdateDataInLeaderboardAwait(
            new List<string>() { "HairRank" + CheckInterval(interval) }, new List<double>() { hairChange });
       
        return AVStatisticToDictionary(avStatistics);
    }


    /// <summary>
    /// 获取 hair 排行榜结果，默认为日榜  获取指定区间的排名,头像,昵称,等级,成绩
    /// </summary>
    /// <param name="start">最小为1</param>
    /// <param name="end">右区间 闭区间</param>
    /// <param name="interval">D, W, M</param>
    /// <returns>interval->List<RankEntry>  D->日榜 W->周榜 M->月榜</returns>
    public static async Task<List<RankEntry>> GetHairRank(int start, int end, string interval = "D")
    {
        return AVRankingToRankEntry(await LeanCloudUtil.GetLeaderboardAwait("HairRank" + CheckInterval(interval), start, end));
    }


    /// <summary>
    /// 获取当前用户附近的排名   hair 排行榜  默认为日榜
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="limit">限制返回的结果数量，当前用户会在结果的中间位置</param>
    public static async Task<List<RankEntry>> GetHairRankAroudMe(string interval = "D", int limit = 1)
    {
        return AVRankingToRankEntry(await LeanCloudUtil.GetLeaderboardAroudMeAwait("HairRank" + CheckInterval(interval), limit));
    }

    private static string CheckInterval(string interval)
    {
        interval = interval.ToUpper();
        if (!(interval.Equals("D") || interval.Equals("W") || interval.Equals("M")))
        {
            interval = "D";
        }
        return interval;
    }
}
