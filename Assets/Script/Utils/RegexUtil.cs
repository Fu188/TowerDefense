using System;
using System.Text.RegularExpressions;

// 来源于网络，魔改并自适应

public class RegexUtil
{
    private static readonly object locker = new object();

    private static RegexUtil _instance;

    private RegexUtil() { }

    public static RegexUtil GetRegexUtilInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new RegexUtil();
                }
            }
        }
        return _instance;
    }

    /// <summary>
    /// 判断输入的字符串只包含汉字
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsChineseCh(string input)
    {
        return IsMatch(@"^[\u4e00-\u9fa5]+$", input);
    }

    /// <summary>
    /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
    /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
    /// 也可以没有间隔
    /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsPhone(string input)
    {
        string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
        return IsMatch(pattern, input);
    }

    /// <summary>
    /// 判断输入的字符串是否是一个合法的手机号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsMobilePhone(string input)
    {
        return IsMatch(@"^13\\d{9}$", input);
    }

    /// <summary>
    /// 判断输入的字符串只包含数字
    /// 可以匹配整数和浮点数
    /// ^-?\d+$|^(-?\d+)(\.\d+)?$
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNumber(string input)
    {
        string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
        return IsMatch(pattern, input);
    }

    /// <summary>
    /// 判断是否为包含 n位数字 的字符串
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool NumCount(string input, int n)
    {
        string pattern = "^\\d{"+n+"}$";
        UnityEngine.Debug.Log(pattern);
        return IsMatch(pattern, input);
    }
    

    /// <summary>
    /// 匹配非负整数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNotNagtive(string input)
    {
        return IsMatch(@"^\d+$", input);
    }
    /// <summary>
    /// 匹配正整数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsUint(string input)
    {
        return IsMatch(@"^[0-9]*[1-9][0-9]*$", input);
    }

    /// <summary>
    /// 判断输入的字符串字包含英文字母
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEnglisCh(string input)
    {
        return IsMatch(@"^[A-Za-z]+$", input);
    }


    /// <summary>
    /// 判断输入的字符串是否是一个合法的Email地址
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmail(string input)
    {
        string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        return IsMatch(pattern, input);
    }


    /// <summary>
    /// 判断输入的字符串是否只包含数字和英文字母
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNumAndEnCh(string input)
    {
        return IsMatch(@"^[A-Za-z0-9]+$", input);
    }


    /// <summary>
    /// 判断输入的字符串是否是一个超链接
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsURL(string input)
    {
        string pattern = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
        return IsMatch(pattern, input);
    }


    /// <summary>
    /// 判断输入的字符串是否是表示一个IP地址
    /// </summary>
    /// <param name="input">被比较的字符串</param>
    /// <returns>是IP地址则为True</returns>
    public static bool IsIPv4(string input)
    {
        string[] IPs = input.Split('.');

        for (int i = 0; i < IPs.Length; i++)
        {
            if (!IsMatch(@"^\d+$", IPs[i]))
            {
                return false;
            }
            if (Convert.ToUInt16(IPs[i]) > 255)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 判断输入的字符串是否是合法的IPV6 地址
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsIPV6(string input)
    {
        string pattern = "";
        string temp = input;
        string[] strs = temp.Split(':');
        if (strs.Length > 8)
        {
            return false;
        }
        int count = RegexUtil.GetStringCount(input, "::");
        if (count > 1)
        {
            return false;
        }
        else if (count == 0)
        {
            pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";
            return IsMatch(pattern, input);
        }
        else
        {
            pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
            return IsMatch(pattern, input);
        }
    }

    #region 正则的通用方法
    /// <summary>
    /// 计算字符串的字符长度，一个汉字字符将被计算为两个字符
    /// </summary>
    /// <param name="input">需要计算的字符串</param>
    /// <returns>返回字符串的长度</returns>
    public static int GetCount(string input)
    {
        return Regex.Replace(input, @"[\u4e00-\u9fa5/g]", "aa").Length;
    }

    /// <summary>
    /// 调用Regex中IsMatch函数实现一般的正则表达式匹配
    /// </summary>
    /// <param name="pattern">要匹配的正则表达式模式。</param>
    /// <param name="input">要搜索匹配项的字符串</param>
    /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
    public static bool IsMatch(string pattern, string input)
    {
        if (input == null || input == "") return false;
        Regex regex = new Regex(pattern);
        return regex.IsMatch(input);
    }

    /// <summary>
    /// 从输入字符串中的第一个字符开始，用替换字符串替换指定的正则表达式模式的所有匹配项。
    /// </summary>
    /// <param name="pattern">模式字符串</param>
    /// <param name="input">输入字符串</param>
    /// <param name="replacement">用于替换的字符串</param>
    /// <returns>返回被替换后的结果</returns>
    public static string Replace(string pattern, string input, string replacement)
    {
        Regex regex = new Regex(pattern);
        return regex.Replace(input, replacement);
    }

    /// <summary>
    /// 在由正则表达式模式定义的位置拆分输入字符串。
    /// </summary>
    /// <param name="pattern">模式字符串</param>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static string[] Split(string pattern, string input)
    {
        Regex regex = new Regex(pattern);
        return regex.Split(input);
    }

    /* *******************************************************************
     * 1、通过“:”来分割字符串看得到的字符串数组长度是否小于等于8
     * 2、判断输入的IPV6字符串中是否有“::”。
     * 3、如果没有“::”采用 ^([\da-f]{1,4}:){7}[\da-f]{1,4}$ 来判断
     * 4、如果有“::” ，判断"::"是否止出现一次
     * 5、如果出现一次以上 返回false
     * 6、^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$
     * ******************************************************************/
    /// <summary>
    /// 判断字符串compare 在 input字符串中出现的次数
    /// </summary>
    /// <param name="input">源字符串</param>
    /// <param name="compare">用于比较的字符串</param>
    /// <returns>字符串compare 在 input字符串中出现的次数</returns>
    private static int GetStringCount(string input, string compare)
    {
        int index = input.IndexOf(compare);
        if (index != -1)
        {
            return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
        }
        else
        {
            return 0;
        }

    }


    #endregion
}


/*
 * C#常用正则表达式

一、校验数字的表达式
 1 数字：^[0-9]*$
 2 n位的数字：^\d{n}$
 3 至少n位的数字：^\d{n,}$
 4 m-n位的数字：^\d{m,n}$
 5 零和非零开头的数字：^(0|[1-9][0-9]*)$
 6 非零开头的最多带两位小数的数字：^([1-9][0-9]*)+(.[0-9]{1,2})?$
 7 带1-2位小数的正数或负数：^(\-)?\d+(\.\d{1,2})?$
 8 正数、负数、和小数：^(\-|\+)?\d+(\.\d+)?$
 9 有两位小数的正实数：^[0-9]+(.[0-9]{2})?$
10 有1~3位小数的正实数：^[0-9]+(.[0-9]{1,3})?$
11 非零的正整数：^[1-9]\d*$ 或 ^([1-9][0-9]*){1,3}$ 或 ^\+?[1-9][0-9]*$
12 非零的负整数：^\-[1-9][]0-9"*$ 或 ^-[1-9]\d*$
13 非负整数：^\d+$ 或 ^[1-9]\d*|0$
14 非正整数：^-[1-9]\d*|0$ 或 ^((-\d+)|(0+))$
15 非负浮点数：^\d+(\.\d+)?$ 或 ^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$
16 非正浮点数：^((-\d+(\.\d+)?)|(0+(\.0+)?))$ 或 ^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$
17 正浮点数：^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$ 或 ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$
18 负浮点数：^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$ 或 ^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$
19 浮点数：^(-?\d+)(\.\d+)?$ 或 ^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$
二、校验字符的表达式
 1 汉字：^[\u4e00-\u9fa5]{0,}$
 2 英文和数字：^[A-Za-z0-9]+$ 或 ^[A-Za-z0-9]{4,40}$
 3 长度为3-20的所有字符：^.{3,20}$
 4 由26个英文字母组成的字符串：^[A-Za-z]+$
 5 由26个大写英文字母组成的字符串：^[A-Z]+$
 6 由26个小写英文字母组成的字符串：^[a-z]+$
 7 由数字和26个英文字母组成的字符串：^[A-Za-z0-9]+$
 8 由数字、26个英文字母或者下划线组成的字符串：^\w+$ 或 ^\w{3,20}$
 9 中文、英文、数字包括下划线：^[\u4E00-\u9FA5A-Za-z0-9_]+$
10 中文、英文、数字但不包括下划线等符号：^[\u4E00-\u9FA5A-Za-z0-9]+$ 或 ^[\u4E00-\u9FA5A-Za-z0-9]{2,20}$
11 可以输入含有^%&',;=?$\"等字符：[^%&',;=?$\x22]+
12 禁止输入含有~的字符：[^~\x22]+
三、特殊需求表达式
 1 Email地址：^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$
 2 域名：[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(/.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+/.?
 3 InternetURL：[a-zA-z]+://[^\s]* 或 ^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$
 4 手机号码：^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$
 5 电话号码("XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX)：^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$ 
 6 国内电话号码(0511-4405222、021-87888822)：\d{3}-\d{8}|\d{4}-\d{7}
 7 身份证号(15位、18位数字)：^\d{15}|\d{18}$
 8 短身份证号码(数字、字母x结尾)：^([0-9]){7,18}(x|X)?$ 或 ^\d{8,18}|[0-9x]{8,18}|[0-9X]{8,18}?$
 9 帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$
10 密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)：^[a-zA-Z]\w{5,17}$
11 强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间)：^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$  
12 日期格式：^\d{4}-\d{1,2}-\d{1,2}
13 一年的12个月(01～09和1～12)：^(0?[1-9]|1[0-2])$
14 一个月的31天(01～09和1～31)：^((0?[1-9])|((1|2)[0-9])|30|31)$ 
15 钱的输入格式：
16    1.有四种钱的表示形式我们可以接受:"10000.00" 和 "10,000.00", 和没有 "分" 的 "10000" 和 "10,000"：^[1-9][0-9]*$ 
17    2.这表示任意一个不以0开头的数字,但是,这也意味着一个字符"0"不通过,所以我们采用下面的形式：^(0|[1-9][0-9]*)$ 
18    3.一个0或者一个不以0开头的数字.我们还可以允许开头有一个负号：^(0|-?[1-9][0-9]*)$ 
19    4.这表示一个0或者一个可能为负的开头不为0的数字.让用户以0开头好了.把负号的也去掉,因为钱总不能是负的吧.下面我们要加的是说明可能的小数部分：^[0-9]+(.[0-9]+)?$ 
20    5.必须说明的是,小数点后面至少应该有1位数,所以"10."是不通过的,但是 "10" 和 "10.2" 是通过的：^[0-9]+(.[0-9]{2})?$ 
21    6.这样我们规定小数点后面必须有两位,如果你认为太苛刻了,可以这样：^[0-9]+(.[0-9]{1,2})?$ 
22    7.这样就允许用户只写一位小数.下面我们该考虑数字中的逗号了,我们可以这样：^[0-9]{1,3}(,[0-9]{3})*(.[0-9]{1,2})?$ 
23    8.1到3个数字,后面跟着任意个 逗号+3个数字,逗号成为可选,而不是必须：^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(.[0-9]{1,2})?$ 
24    备注：这就是最终结果了,别忘了"+"可以用"*"替代如果你觉得空字符串也可以接受的话(奇怪,为什么?)最后,别忘了在用函数时去掉去掉那个反斜杠,一般的错误都在这里
25 xml文件：^([a-zA-Z]+-?)+[a-zA-Z0-9]+\\.[x|X][m|M][l|L]$
26 中文字符的正则表达式：[\u4e00-\u9fa5]
27 双字节字符：[^\x00-\xff]    (包括汉字在内，可以用来计算字符串的长度(一个双字节字符长度计2，ASCII字符计1))
28 空白行的正则表达式：\n\s*\r    (可以用来删除空白行)
29 HTML标记的正则表达式：<(\S*?)[^>]*>.*?</\1>|<.*? />    (网上流传的版本太糟糕，上面这个也仅仅能部分，对于复杂的嵌套标记依旧无能为力)
30 首尾空白字符的正则表达式：^\s*|\s*$或(^\s*)|(\s*$)    (可以用来删除行首行尾的空白字符(包括空格、制表符、换页符等等)，非常有用的表达式)
31 腾讯QQ号：[1-9][0-9]{4,}    (腾讯QQ号从10000开始)
32 中国邮政编码：[1-9]\d{5}(?!\d)    (中国邮政编码为6位数字)
33 IP地址：\d+\.\d+\.\d+\.\d+    (提取IP地址时有用)

*/