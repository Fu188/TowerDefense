using System;

/// <summary>
/// 日期采用 ISO8601 格式，中国标准时间
/// </summary>
public class TimeUtil
{


    // <summary>
    /// 日期格式 ISO8601  yyyy-MM-ddTHH:mm:ss.fffzzzz 例如 2020-10-18T12:27:24.438+08:00
    /// </summary>
    public static readonly string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzzz";


    private static readonly object locker = new object();

    private static TimeUtil _instance;

    private TimeUtil() { }

    public static TimeUtil GetTimeUtilInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new TimeUtil();
                }
            }
        }
        return _instance;
    }


    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 获取当前中国标准时间
    /// </summary>
    /// <returns>DateTime 中国标准时间</returns>
    public static DateTime Now()
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "China Standard Time"); //中国标准时间
        
    }


    /// <summary>
    /// 获取当前中国标准时间
    /// </summary>
    /// <returns>string 中国标准时间</returns>
    public static string NowInString()
    {
        DateTime chinaDT = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "China Standard Time"); //中国标准时间
        return chinaDT.ToString(DateTimeFormat);
    }


    /// <summary>
    /// 获取当前时间之后的某一刻时间
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="unit">s: second, m(默认值): minute, H: hour, d: day, y: year</param>
    /// <returns>DateTime</returns>
    public static DateTime AfterNow(int value, char unit = 'm')
    {
        DateTime target = Now();
        switch (unit)
        {
            case 's':
                target = target.AddSeconds(value);
                break;
            case 'm':
                target = target.AddMinutes(value);
                break;
            case 'H':
                target = target.AddHours(value);
                break;
            case 'd':
                target = target.AddDays(value);
                break;
            case 'y':
                target = target.AddYears(value);
                break;
            default:
                target = target.AddMinutes(value);
                break;
        }
        return target;
    }



    /// <summary>
    /// 获取当前时间之后的某一刻时间
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="unit">s: second, m(默认值): minute, H: hour, d: day, y: year</param>
    /// <returns>string</returns>
    public static string AfterNowInString(int value, char unit = 'm')
    {
        DateTime chinaDT = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(AfterNow(value,unit), "China Standard Time"); //中国标准时间
        return chinaDT.ToString(DateTimeFormat);
    }




    /// <summary>
    /// 中国标准时间 DateTime 转 string
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>string</returns>
    public static string DateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString(DateTimeFormat);
    }

    /// <summary>
    /// 中国标准时间 string 转 DateTime
    /// </summary>
    /// <param name="dateTimeString"></param>
    /// <returns>DateTime</returns>
    public static DateTime StringToDateTime(string dateTimeString)
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(dateTimeString), "China Standard Time");
        //return DateTime.Parse(dateTimeString); // Convert.ToDateTime(dateTimeString)
    }


    /// <summary>
    /// 计算两个日期时间的差值，结果以分钟为单位
    /// </summary>
    /// <returns>doube 正值为 d1 > d2</returns>
    public static double CalculateDiff(DateTime d1, DateTime d2)
    {
        return (d1 - d2).TotalMinutes;
    }

    /// <summary>
    /// 计算两个日期时间的差值，结果以分钟为单位
    /// </summary>
    /// <returns>doube 正值为 d1 > d2</returns>
    public static double CalculateDiff(DateTime d1, string d2)
    {
        return (d1 - DateTime.Parse(d2)).TotalMinutes;
    }

    /// <summary>
    /// 计算两个日期时间的差值，结果以分钟为单位
    /// </summary>
    /// <returns>doube 正值为 d1 > d2</returns>
    public static double CalculateDiff(string d1, DateTime d2)
    {
        return (DateTime.Parse(d1) - d2).TotalMinutes;
    }

    /// <summary>
    /// 计算两个日期时间的差值，结果以分钟为单位
    /// </summary>
    /// <returns>doube 正值为 d1 > d2</returns>
    public static double CalculateDiff(string d1, string d2)
    {
        return (DateTime.Parse(d1) - DateTime.Parse(d2)).TotalMinutes;
    }

    /// <summary>
    /// 上次到现在的时间差值，返回值单位为分钟
    /// </summary>
    /// <returns>double</returns>
    public static double LastTimeToNow(DateTime lastTime)
    {
        return CalculateDiff(Now(), lastTime);
    }


    /// <summary>
    /// 上次到现在的时间差值，返回值单位为分钟
    /// </summary>
    /// <returns>double</returns>
    public static double LastTimeToNow(string lastTime)
    {
        return CalculateDiff(Now(), lastTime);
    }


    public static string FormatTimeString(string dateTimeString, string format = "yyyy/MM/dd HH:mm")
    {
        //TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "China Standard Time").ToString(format);
        return DateTime.Parse(dateTimeString).ToString(format);
    }
}
