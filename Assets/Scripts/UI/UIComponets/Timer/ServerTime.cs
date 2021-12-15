using System;
using System.Text;
using UnityEngine;

namespace ProHA
{
    public class ServerTime : MonoBehaviour
{
    /// <summary>
    /// 游戏启动时间 /毫秒
    /// </summary>
    private static long ServerTimeStartup;
    /// <summary>
    /// 时区，按理说应该服务器给
    /// </summary>
    public static int Timezone = 8;
    private static ServerTime Instance = null;
    private DateTime lastTime = DateTime.MaxValue;
    private static long ServerT;

    private static long ServerOpenTime;
    public static int ServerOpenDays {get; private set;}
    
    public static event Action<DateTime> OnTick;
    public static event Action<DateTime> OnCrossHour;
    public static event Action<DateTime> OnCrossDay;
    public static event Action<DateTime> OnCrossMin;
    public static event Action<DateTime> OnCrossTenMin;//十分钟
    public static void Init()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("ServerTime");
            DontDestroyOnLoad(obj);
            Instance = obj.AddComponent<ServerTime>();
        }
    }
    public static void Init(long serverTime)
    {
        ServerT = serverTime;
    }

    public static void InitServerOpenTime(long time)
    {
        ServerOpenTime = time;
        ServerOpenDays = DaysFromServerOpen();
        LC.SendEvent("UPDATE_SERVER_OPEN_TIME");
    }
    void Start()
    {
        InvokeRepeating("TickHandle", 0, 1);
    }

    private void Update()
    {
        if (ServerT > 0)
        {
            ServerTimeStartup = ServerT - GetPassTime();
            ServerT = 0;
        }
    }

    void TickHandle()
    {
        if (ServerTimeStartup<=0)
        {
            return;
        }
        DateTime nowTime = GetNowTime;
        if (OnTick != null) OnTick(nowTime);
        if( lastTime == DateTime.MaxValue )
        {
            lastTime = nowTime;
            return;
        }
        if (nowTime.Hour != lastTime.Hour)
        {
            Invoke("SendCrossHour", 2);           
        }
        if (nowTime.Day != lastTime.Day)
        {
            Invoke("SendCrossDay", 2);
        }
        if (nowTime.Minute % 10 == 0 && lastTime.Minute % 10 != 0)
        {
            Invoke("SendCrossTenMin", 2);
        }        
        if (nowTime.Minute!=lastTime.Minute)
        {
            Invoke("SendCrossMin", 2);
        }
        lastTime = nowTime;
    }
    private void SendCrossMin()
    {
        OnCrossMin?.Invoke(GetNowTime);
        LC.SendEvent("CROSS_ONE_MIN");
    }
    private void SendCrossTenMin()
    {
        OnCrossTenMin?.Invoke(GetNowTime);
        LC.SendEvent("CROSS_TEN_MIN");
    }
    private void SendCrossHour()
    {
        OnCrossHour?.Invoke(GetNowTime);
        LC.SendEvent("CROSS_HOUR");
    }

    private void SendCrossDay()
    {
        OnCrossDay?.Invoke(GetNowTime);
        ServerOpenDays = DaysFromServerOpen();//跨天时重新计算开服天数
        LC.SendEvent("CROSS_DAY");
    }
    void OnDestroy()
    {
        CancelInvoke();
    }
    /// <summary>
    /// 当前服务器时间 /毫秒
    /// </summary>
    /// <returns></returns>
    public static long CurrentTime()
    {
        return ServerTimeStartup + GetPassTime();
    }

    /// <summary>
    /// 游戏启动开始到现在多消耗时间 /毫秒
    /// </summary>
    /// <returns></returns>
    public static long GetPassTime()
    {
        return (long)(Time.realtimeSinceStartup * 1000);
    }

    /// <summary>
    ///  当前时间到目标时间的时间间隔 /秒
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    public static long CountDown(long targetTime)
    {
        return CountDownMilli(targetTime) / 1000;
    }
    /// <summary>
    ///  当前时间到目标时间的时间间隔 /毫秒
    /// </summary>
    /// <param name="targetTime"></param>
    /// <returns></returns>
    public static long CountDownMilli(long targetTime)
    {
        return targetTime - CurrentTime();
    }
    /// <summary>
    /// 得到C#的时间刻度 毫秒转成100纳秒表示
    /// </summary>
    /// <param name="res"> /毫秒</param>
    /// <returns></returns>
    public static long GetUnitTicks(long res)
    {
        return res * 10000;
    }
    /// <summary>
    /// 得到一个时间段显示返回  01:59:01
    /// </summary>
    /// <param name="targetTime">单位毫秒</param>
    /// <returns></returns>
    public static string GetStriTime(long targetTime)
    {
        long chaTime = ServerTime.CountDown(targetTime);
        string str = FormatTime(chaTime);
        return str;
    }
    /// <summary>
    /// 得到一个时间段显示返回  59
    /// </summary>
    /// <param name="targetTime">单位毫秒</param>
    /// <returns></returns>
    public static string GetStriTimeS(long targetTime)
    {
        long chaTime = ServerTime.CountDown(targetTime);
        string str = FormatTimeS(chaTime);
        return str;
    }
    /// <summary>
    /// 返回  01:59:01
    /// </summary>
    /// <param name="time">单位s</param>
    /// <returns></returns>
    public static string FormatTime(long time)
    {
        string str = "00:00:00";
        if (time > 0)
        {
            long h = time / 3600;
            long m = (time % 3600) / 60;
            long s = time % 60;
            str = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", h, m, s);
        }
        return str;

    }
    /// <summary>
    /// 返回 1天1小时1分
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    //time : 单位s
    public static string FormatTimeD(long time)
    {
        string str = "";
        if (time > 0)
        {
            long d = time / 3600 / 24;
            time = time - d * 3600 * 24;
            long h = time / 3600;
            time = time - h * 3600;
            long m = time / 60 + 1;
            if (d > 0) str = d + "天";
            if (str != "" || h > 0) str += h + "小时";
            if (str != "" || m > 0) str += m + "分";
        }
        return str;

    }
        /// <summary>
        /// 返回 1天1小时 或 1小时1分
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatTimeShort(long time)
        {
            string str = "";
            if (time > 0)
            {
                long d = time / 3600 / 24;
                time = time - d * 3600 * 24;
                long h = time / 3600;
                time = time - h * 3600;
                long m = time / 60 ;
                long s = time - m * 60;
                int textNum = 0;
                if (d > 0)
                {
                    str = d + "天";
                    textNum++;
                }
                if ( h > 0)
                {
                    str += h + "小时";
                    textNum++;
                }
                if (m > 0 && textNum < 2)
                {
                    str += m + "分";
                    textNum++;
                }
                if(s > 0 && textNum < 2)
                {
                    str += s + "秒";
                }
            }
            return str;

        }
        /// <summary>
        /// 返回 1天1小时 或 1小时1分
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatTimeShortWithoutS(long time)
        {
            string str = "";
            if (time > 0)
            {
                long d = time / 3600 / 24;
                time = time - d * 3600 * 24;
                long h = time / 3600;
                time = time - h * 3600;
                long m = time / 60 ;
                int textNum = 0;
                if (d > 0)
                {
                    str = d + "天";
                    textNum++;
                }
                if ( h > 0)
                {
                    str += h + "小时";
                    textNum++;
                }
                if (m > 0 && textNum < 2)
                {
                    str += m + "分";
                    textNum++;
                }
            }
            return str;

        }
        /// <summary>
        /// 按距离天数显示不同格式
        /// </summary>
        /// <param name="timeStamp">服务器时间戳</param>
        /// <returns></returns>
        public static string FormatTimeStamp(ulong timeStamp)
        {
            DateTime dt = GetStandardTime(Convert.ToInt64( timeStamp));
            int diffDays = (DateTime.Now - dt).Days;
            string s;
            if (diffDays >= 7)
            {
                s = dt.ToString("yyyy-MM-dd");
            }
            else if (diffDays == 0)
            {
                s = dt.ToString("HH:mm");
            }
            else
            {
                s = dt.ToString("dddd HH:mm");
            }
            //string.Format("{0:yyyy年MM月dd日  HH:mm:ss dddd,MMMM}", dt);
            return s;
        }
        //time : 单位s
        public static string FormatTimeS(long time)
    {
        string str = "00";
        if (time > 0)
        {
            long s = time % 60;
            str = string.Format("{0,2:D2}", s);
        }
        return str;

    }
    /// <summary>
    /// 换算当前服务器具体时间为纳秒*
    /// </summary>
    /// <param name="res">毫秒</param>
    /// <returns></returns>

    public static long GetNanosecond(long res)
    {
        DateTime startTime = new DateTime(1970, 1, 1);
        startTime = startTime.AddHours(Timezone);
        long startLo = startTime.ToBinary();

        return res * 10000 + startLo;
    }

    public static long GetStondMillisecond(long Ticks)
    {
        DateTime startTime = new DateTime(1970, 1, 1);
        startTime = startTime.AddHours(Timezone);
        long startLo = startTime.ToBinary();
        long Lo = (Ticks - startLo) / 10000;
        return Lo;
    }

    /// <summary>
    /// 得到具体时间对象
    /// </summary>
    /// <param name="res">毫秒</param>
    /// <returns></returns>

    public static DateTime GetStandardTime(long res)
    {
        long Nanosecond = GetNanosecond(res);
        DateTime startTime = new DateTime(Nanosecond);
        return startTime;
    }
    /// <summary>
    /// 判断两个时间戳是否为同一天
    /// </summary>
    /// <param name="t1">毫秒</param>
    /// <param name="t2">毫秒</param>
    /// <returns></returns>
    public static bool IsSameDay(long t1,long t2)
    {
        DateTime dt1 = GetStandardTime(t1);
        DateTime dt2 = GetStandardTime(t2);
        return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day;
    }
    /// <summary>
    /// 当前具体时间  年月日hsm
    /// </summary>
    /// <returns></returns>

    public static DateTime GetNowTime
    {
        get { return GetStandardTime(CurrentTime()); }
    }

    public static long GetServerTime(int hour, int minute = 0, int second = 0)
    {
        var now = GetNowTime;
        return CurrentTime() - ((now.Hour - hour) * 3600 + (now.Minute - minute) * 60 + (now.Second - second)) * 1000;
    }
    
    public static string FormartTargetTime(long targetTime)
    {
        var time = GetStandardTime(targetTime);
        var str = string.Format("{0}/{1,2:D2}/{2,2:D2}", time.Year, time.Month, time.Day);
        return str;
    }

    public static string FormartTargetTimeHMS(long targetTime, string separator = ":")
    {
        var time = GetStandardTime(targetTime);
        var str = string.Format("{0}{3}{1,2:D2}{3}{2,2:D2}", time.Hour, time.Minute, time.Second, separator);
        return str;
    }
    public static string FormartTargetTimeHM(long targetTime)
    {
        var time = GetStandardTime(targetTime);
        int hour = time.Hour;
        int min = time.Minute;
        string minS = min < 10 ? "0" + min : min.ToString();
        return hour + ":" + minS;
    }

    public static string FormartTargetTimeYYMMDDHMS(long targetTime,bool year2)
    {
        var time = GetStandardTime(targetTime);
        var str1 = string.Format("{0}/{1,2:D2}/{2,2:D2}", year2?(time.Year%100):time.Year, time.Month, time.Day);
        var str2 = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", time.Hour, time.Minute, time.Second);
        return str1+" "+str2;
    }
    public static bool CheckIsSameDay(int time)
    {
        bool b = false;
        long t = (long)time * 1000;
        DateTime target = GetStandardTime(t);
        if ( GetNowTime.Year == target.Year && GetNowTime.Month == target.Month && GetNowTime.Day == target.Day )
        {
            return true;
        }
        return b;
    }

    public static int DaysFromServerOpen()
    {//开服天数
        return CountDays(GetStandardTime(ServerOpenTime), GetNowTime);
    }
    
    public static int CountDays(DateTime createTime,DateTime curTimeT)
    {
        DateTime createTime2 = createTime.Date;
        DateTime startT = new DateTime(createTime2.Year, createTime2.Month, createTime2.Day, 18, 0, 0);
        DateTime endT = new DateTime(curTimeT.Year, curTimeT.Month, curTimeT.Day, 18, 0, 0);
        TimeSpan span = endT - startT;
        return span.Days + 1;
    }
    
}

}
