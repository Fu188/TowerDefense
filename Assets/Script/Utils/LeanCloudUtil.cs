using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud;
using static LeanCloud.AVClient;

/// <summary>
/// https://leancloud.cn/docs/unity_guide.html
/// https://leancloud.cn/docs/leaderboard-guide-dotnet.html
/// 
/// Await 后缀的方法会阻塞进程
/// </summary>
public class LeanCloudUtil
{
    static Configuration config = new Configuration();
    const string defaultAvatar = "https://keykeeper.ga:8080/static/img/user/avatar/default.jpg";

    static List<string> UserKeys = new List<string> { "nickName", "avatar", "level" };
    static LeanCloudUtil()
    {
        // 开启存储日志
        //AVClient.HttpLog(UnityEngine.Debug.Log);
        bool testing = true;
        config.ApplicationId = "c78tB3QqxJwd8hBlxCnDzlny-MdYXbMMI";
        config.ApplicationKey = "msW89o6sNScSiSDxLQsUOg1L";
        config.Region = Configuration.AVRegion.Public_North_America;
        if (testing)
        {
            config.ApplicationId = "I9erzW1PMkt7Q0vseOe2bS3P-MdYXbMMI";
            config.ApplicationKey = "NakKw9OM73kFyKHJ0SnzIQfq";
        }
        Initialize(config);
    }


    public static async Task<bool> SignUpAwait(string nickName, string pwd, string email, string avatar = defaultAvatar)
    {
        AVUser user = new AVUser
        {
            Username = email,
            Password = pwd,
            Email = email
        };
        await user.SignUpAsync();
        user["nickName"] = nickName;
        user["avatar"] = avatar;
        user["level"] = 1;
        await user.SaveAsync();
        return true;
    }


    public static void LogIn(string email, string pwd)
    {
        AVUser.LogInByEmailAsync(email, pwd);
    }


    public static async Task<bool> LogInAwait(string email, string pwd)
    {
        AVUser user = await AVUser.LogInByEmailAsync(email, pwd);
        //print(user["nickName"] + " login success, email: " + user.Email);
        return true;
    }


    public static AVUser GetCurrentUser()
    {
        return AVUser.CurrentUser;
    }


    public static void ChangeNickName(string nickName)
    {
        AVUser.CurrentUser["nickName"] = nickName;
        AVUser.CurrentUser.SaveAsync();
    }

    public static async Task<bool> ChangeNickNameAwait(string nickName)
    {
        AVUser.CurrentUser["nickName"] = nickName;
        await AVUser.CurrentUser.SaveAsync();
        return true;
    }

    public static void ChangeAvatar(string avatar)
    {
        AVUser.CurrentUser["avatar"] = avatar;
        AVUser.CurrentUser.SaveAsync();
    }

    public static async Task<bool> ChangeAvatarAwait(string avatar)
    {
        AVUser.CurrentUser["avatar"] = avatar;
        await AVUser.CurrentUser.SaveAsync();
        return true;
    }

    public static void ChangeLevel(int level)
    {
        AVUser.CurrentUser["level"] = level;
        AVUser.CurrentUser.SaveAsync();
    }

    public static async Task<bool> ChangeLevelAwait(int level)
    {
        AVUser.CurrentUser["level"] = level;
        await AVUser.CurrentUser.SaveAsync();
        return true;
    }


    public static void LogOut()
    {
        AVUser.LogOut();
    }



    /// <summary>
    /// 更新排行榜中的成绩
    /// </summary>
    /// <param name="name">排行榜名字</param>
    /// <param name="data">成绩数据</param>
    public static void UpdateDataInLeaderboard(string name, double data)
    {
        Dictionary<string, double> statistic = new Dictionary<string, double>();
        statistic.Add(name, data);
        AVLeaderboard.UpdateStatistics(AVUser.CurrentUser, statistic);
    }

    /// <summary>
    /// 更新多个排行榜中的成绩, 两个参数list 大小应该一致
    /// </summary>
    /// <param name="names">排行榜名字数组</param>
    /// <param name="datas">成绩数据数组</param>
    public static void UpdateDataInLeaderboard(List<string> leaderBoradNames, List<double> datas)
    {
        int len = leaderBoradNames.Count;
        if (len == datas.Count)
        {
            Dictionary<string, double> statistic = new Dictionary<string, double>();
            for (int i = 0; i < len; ++i)
            {
                statistic.Add(leaderBoradNames[i], datas[i]);

            }
            AVLeaderboard.UpdateStatistics(AVUser.CurrentUser, statistic);
        }
    }


    /// <summary>
    /// 更新多个排行榜中的成绩, 两个参数数组长度应该一致
    /// </summary>
    /// <param name="names">排行榜名字数组</param>
    /// <param name="datas">成绩数据数组</param>
    public static async Task<List<AVStatistic>> UpdateDataInLeaderboardAwait(List<string> leaderBoradNames, List<double> datas)
    {
        int len = leaderBoradNames.Count;
        
        if (len == datas.Count)
        {
            Dictionary<string, double> statistic = new Dictionary<string, double>();
            for (int i = 0; i < len; ++i)
            {
                statistic.Add(leaderBoradNames[i], datas[i]);

            }
            return await AVLeaderboard.UpdateStatistics(AVUser.CurrentUser, statistic);

        }
        return new List<AVStatistic>();
    }



    public delegate void OnGotDataInLeaderboard(Dictionary<string, double> leaderboardData);

    /// <summary>
    /// 查询排行榜的成绩
    /// </summary>
    /// <param name="call">回调函数，回调参数为 Dictionary<string, double> </param>
    /// <param name="leaderboardNames">排行榜名字，若为空，将查询所有排行榜成绩</param>
   /* public static async void GetDataInLeaderboard(OnGotDataInLeaderboard call, List<string> leaderboardNames = null)
    {
        // 构造要查询的用户
        //AVUser otherUser = AVObject.CreateWithoutData<AVUser>(objectId);
        // 获取其他用户在排行榜中的成绩
        var statistics = await AVLeaderboard.GetStatistics(AVUser.CurrentUser, leaderboardNames);
        Dictionary<string, double> leaderboardData = new Dictionary<string, double>();
        foreach (var statistic in statistics)
        {
            leaderboardData.Add(statistic.Name, statistic.Value);
        }
        call(leaderboardData);
    }*/



    /// <summary>
    /// 查询排行榜的成绩
    /// </summary>
    /// <param name="call">回调函数，回调参数为 Dictionary<string, double> </param>
    /// /// <param name="leaderboardNames">为空时返回所有排行榜成绩</param>
    public static async void GetDataInLeaderboard(OnGotDataInLeaderboard call, params string[] leaderboardNames)
    {
        var statistics = await AVLeaderboard.GetStatistics(AVUser.CurrentUser, leaderboardNames.Length == 0 ? null : new List<string>(leaderboardNames));
        Dictionary<string, double> leaderboardData = new Dictionary<string, double>();
        foreach (var statistic in statistics)
        {
            leaderboardData.Add(statistic.Name, statistic.Value);
        }
        call(leaderboardData);
    }


    /// <summary>
    /// 查询排行榜的成绩
    /// </summary>
    /// <param name="call">回调函数，回调参数为 Dictionary<string, double> </param>
    /// /// <param name="leaderboardNames">为空时返回所有排行榜成绩</param>
    public static async Task<List<AVStatistic>> GetDataInLeaderboardAwait(params string[] leaderboardNames)
    {
        return await AVLeaderboard.GetStatistics(AVUser.CurrentUser, leaderboardNames.Length == 0 ? null : new List<string>(leaderboardNames));
    }



    public delegate void OnGotLeaderboard(List<AVRanking> avRankings);
    /// <summary>
    /// 获取排行榜结果  获取指定区间的排名,头像,昵称,等级,成绩
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="start">最小为1</param>
    /// <param name="end">右区间 闭区间</param>
    public static async void GetLeaderboard(OnGotLeaderboard call, string leaderboardName, int start, int end)
    {
        AVLeaderboard leaderboard = AVLeaderboard.CreateWithoutData(leaderboardName);
        List<AVRanking> leaderboardResult = await leaderboard.GetResults(limit: end - start + 1, skip: start - 1, selectUserKeys: UserKeys);
        //  includeStatistics: new List<string> { "kills" }
        call(leaderboardResult);
    }



    /// <summary>
    /// 获取排行榜结果  获取指定区间的排名,头像,昵称,等级,成绩
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="start">最小为1</param>
    /// <param name="end">右区间 闭区间</param>
    public static async Task<List<AVRanking>> GetLeaderboardAwait(string leaderboardName, int start, int end)
    {
        AVLeaderboard leaderboard = AVLeaderboard.CreateWithoutData(leaderboardName);
        return await leaderboard.GetResults(limit: end - start + 1, skip: start - 1, selectUserKeys: UserKeys);
        //  includeStatistics: new List<string> { "kills" }
    }




    /// <summary>
    /// 获取当前用户附近的排名
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="limit">限制返回的结果数量，当前用户会在结果的中间位置</param>
    public static async void GetLeaderboardAroudMe(OnGotLeaderboard call, string leaderboardName, int limit = 1)
    {
        AVLeaderboard leaderboard = AVLeaderboard.CreateWithoutData(leaderboardName);
        List<AVRanking> leaderboardResult = await leaderboard.GetResultsAroundUser(limit: limit, selectUserKeys: UserKeys);
        //  includeStatistics: new List<string> { "kills" }
        call(leaderboardResult);
    }



    /// <summary>
    /// 获取当前用户附近的排名
    /// </summary>
    /// <param name="leaderboardName"></param>
    /// <param name="limit">限制返回的结果数量，当前用户会在结果的中间位置</param>
    public static async Task<List<AVRanking>> GetLeaderboardAroudMeAwait(string leaderboardName, int limit = 1)
    {
        AVLeaderboard leaderboard = AVLeaderboard.CreateWithoutData(leaderboardName);
        return await leaderboard.GetResultsAroundUser(limit: limit, selectUserKeys: UserKeys);
    }




    public delegate void OnUploadedFile(string fileUrl);
    public static async void UploadFileAsync(long userId, string fileName, byte[] fileData, OnUploadedFile call)
    {
        AVFile file = new AVFile(fileName, fileData, new Dictionary<string, object>(){
            {"userId",userId}
        });
        await file.SaveAsync();
        call(file.Url.AbsoluteUri);
    }

    public static async Task<string> UploadFileAsyncWithProgress(long userId, string fileName, byte[] fileData)
    {

        AVFile file = new AVFile(fileName, fileData, new Dictionary<string, object>(){
            {"userId",userId}
        });
        await file.SaveAsync(new ProgressListener());

        return file.Url.AbsoluteUri;
    }


    class ProgressListener : System.IProgress<AVUploadProgressEventArgs>
    {
        public void Report(AVUploadProgressEventArgs value)
        {
            UnityEngine.Debug.Log(value.Progress);
        }
    }

}
