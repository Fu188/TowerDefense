
public class UserController
{
    private static UserController _instance;

    // 定义一个标识确保线程同步
    private static readonly object locker = new object();

    private static UserService userService;


    private UserController()
    {
    }


    public static UserController GetUserControllerInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new UserController();
                    userService = UserService.GetUserServiceInstance();
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
        return RegexUtil.IsEmail(email) && userService.GetSignUpCode(email);
    }

    /// <summary>
    /// 获取重置密码验证码
    /// </summary>
    /// <param name="email">用户邮箱</param>
    /// <returns>true: 发送成功; flase: 发送失败或者用户 未注册</returns>
    public bool GetResetPwdCode(string email)
    {
        return RegexUtil.IsEmail(email) && userService.GetResetPwdCode(email);
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public bool SignUp(string email, string pwd, string nickName, string code)
    {
        if (RegexUtil.IsEmail(email) && (!string.IsNullOrEmpty(pwd)) && (!string.IsNullOrEmpty(pwd)) && RegexUtil.NumCount(code, 6) && userService.SignUp(email, pwd, nickName, code))
        {
            LeanCloudUtil.SignUpAwait(nickName, pwd, email);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 用户登陆
    /// </summary>
    public bool SignIn(string email, string pwd)
    {
        if (RegexUtil.IsEmail(email) && (!string.IsNullOrEmpty(pwd)) && userService.SignIn(email, pwd))
        {
            LeanCloudUtil.LogIn(email, pwd);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 通过邮箱验证码重置密码
    /// </summary>
    public bool ResetPwd(string email, string newPwd, string code)
    {
        if (RegexUtil.IsEmail(email) && (!string.IsNullOrEmpty(newPwd)) && RegexUtil.NumCount(code, 6) && userService.ResetPwd(email, newPwd, code))
        {
            //LeanCloudUtil.
            return true;
        }
        return false;
    }

    /// <summary>
    /// 更新用户数据, 修改密码、更新头像 请使用其他方法
    /// </summary>
    /// <remarks>dto: 1 更新你科币、学分、道具、成就; 2 更新头发、最后活跃时间; 3 更新经验值、你科币、学分、卡牌、道具、成就、图鉴;  其他 更新整个user</remarks>
    /// <param name="dto">参数使用: 邮件附件领取(1), 游戏开始(2), 游戏结束(3), 更新整个user(other)</param>
    public bool UpdateUserInfo(int dto)
    {
        User user = User.GetUserInstance();
        bool result = false;
        switch (dto)
        {
            case 1:
                result=  userService.UpdateUserInfo(new UserEmailDto(user));
                break;
            case 2:
                result = userService.UpdateUserInfo(new UserHairTimeDto(user));
                break;
            case 3:
                result =  userService.UpdateUserInfo(new UserGameInfoDto(user));
                break;
            default:
                result = userService.UpdateUserInfo(user);
                break;

        }
        return result;
        
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public bool ChangePwd(long userId, string oldPwd, string newPwd)
    {
        if (userId != 0 && (!string.IsNullOrEmpty(oldPwd)) && (!string.IsNullOrEmpty(newPwd)) && userService.ChangePwd(userId, oldPwd, newPwd))
        {
            //LeanCloudUtil.
            return true;
        }
        return false;
    }

    /// <summary>
    /// 修改头像
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="avatarFilePath">头像图片路径</param>
    /// <returns>如果上传的非图片或者超出限定大小3M，返回默认头像url，否则返回成功上传的图片url</returns>
    public string ChangeAvatar(long userId, string avatarFilePath)
    {
        string newAvatar = userService.ChangeAvatar(userId, avatarFilePath);
        LeanCloudUtil.ChangeAvatar(newAvatar);
        return newAvatar;
    }

    public string ChangeAvatar(long userId, byte[] image)
    {
        string newAvatar = userService.ChangeAvatar(userId, image);
        LeanCloudUtil.ChangeAvatar(newAvatar);
        HomePageManager.GetHomePageManagerInstance().ShowImage(newAvatar);
        return newAvatar;
    }
}