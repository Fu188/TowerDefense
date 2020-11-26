using System;
using Newtonsoft.Json;
using UnityEngine;


public class UserService
{
    //简单的单例（反正是自己用，没人用反射搞我们）
    private static UserService _instance;

    // 定义一个标识确保线程同步
    private static readonly object locker = new object();

    private string baseUrl = NetWorkInfo.backEndBaseUrl + "/v1";


    private UserService()
    {
    }


    public static UserService GetUserServiceInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new UserService();
                }
            }
        }
        return _instance;
    }

    /// <summary>
    /// 获取注册验证码
    /// </summary>
    /// <param name="email">用户邮箱</param>
    /// <returns>true: 发送成功; flase: 发送失败或者用户 已注册</returns>
    public bool GetSignUpCode(string email)
    {
        return HttpUtil.Get(string.Format("{0}/user/getSignUpCode/{1}", baseUrl, email)).Equals("true");
    }

    /// <summary>
    /// 获取重置密码验证码
    /// </summary>
    /// <param name="email">用户邮箱</param>
    /// <returns>true: 发送成功; flase: 发送失败或者用户 未注册</returns>
    public bool GetResetPwdCode(string email)
    {
        return HttpUtil.Get(string.Format("{0}/user/getResetPwdCode/{1}", baseUrl, email)).Equals("true");
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public bool SignUp(string email, string pwd, string nickName, string code)
    {
        pwd = AESUtil.AESEncrypt(pwd);
        string userDto = "{" + string.Format("\"email\":\"{0}\",\"pwd\":\"{1}\",\"nickName\":\"{2}\",\"code\":{3}", email, pwd, nickName, code) + "}";
        return HttpUtil.Post(userDto, string.Format("{0}/user/signUp", baseUrl)).Equals("true");
    }

    /// <summary>
    /// 用户登陆
    /// </summary>
    public bool SignIn(string email, string pwd)
    {
        pwd = AESUtil.AESEncrypt(pwd);
        // example1: System.DateTime.Now method
        DateTime dt1 = DateTime.Now;
        string userDto = "{" + string.Format("\"email\":\"{0}\",\"pwd\":\"{1}\"", email, pwd) + "}";
        //UnityEngine.Debug.Log(pwd+" post url: " + string.Format("{0}/user/signIn", baseUrl));
        string result = HttpUtil.Post(userDto, string.Format("{0}/user/signIn", baseUrl));

        DateTime dt2 = DateTime.Now;

        TimeSpan ts = dt2.Subtract(dt1);
        //Debug.Log(string.Format("Request time consuming: {0} s", ts.TotalSeconds));

        if (string.IsNullOrEmpty(result))
        {
            return false;
        }
        JsonUtility.FromJsonOverwrite(result, User.GetUserInstance());

        return true;
    }

    /// <summary>
    /// 通过邮箱验证码重置密码
    /// </summary>
    public bool ResetPwd(string email, string newPwd, string code)
    {
        newPwd = AESUtil.AESEncrypt(newPwd);
        string userDto = "{" + string.Format("\"email\":\"{0}\",\"newPwd\":\"{1}\",\"code\":{3}", email, newPwd, code) + "}";
        return HttpUtil.Post(userDto, string.Format("{0}/user/resetPwd", baseUrl)).Equals("true");
    }

    /// <summary>
    /// 更新用户数据
    /// </summary>
    /// <remarks>修改密码请使用 Client.ChangePwd(string userId, string oldPwd, string newPwd) 方法</remarks>
    /// <param name="user">User实体类</param>
    public bool UpdateUserInfo(object userDto)
    {
        return HttpUtil.Put(JsonConvert.SerializeObject(userDto), string.Format("{0}/user/updateUserInfo", baseUrl)).Equals("true");
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public bool ChangePwd(long userId, string oldPwd, string newPwd)
    {
        string userIdStr = AESUtil.AESEncrypt(userId);
        oldPwd = AESUtil.AESEncrypt(oldPwd);
        newPwd = AESUtil.AESEncrypt(newPwd);
        string userDto = "{" + string.Format("\"userId\":\"{0}\",\"oldPwd\":\"{1}\",\"newPwd\":\"{2}\"", userIdStr, oldPwd, newPwd) + "}";
        return HttpUtil.Post(userDto, string.Format("{0}/user/changePwd", baseUrl)).Equals("true");
    }

    public string ChangeAvatar(long userId, string avatarFilePath)
    {
        return HttpUtil.PutOneMultipartFile(avatarFilePath, string.Format("{0}/user/chg/avatar/{1}", baseUrl, userId));
    }

    public string ChangeAvatar(long userId, byte[] avatarFile)
    {
        return HttpUtil.PutOneMultipartFile(avatarFile, userId, string.Format("{0}/user/chg/avatar/{1}", baseUrl, userId));
    }
}