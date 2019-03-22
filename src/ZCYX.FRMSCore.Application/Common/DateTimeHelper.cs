using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Common
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 根据日来获取当前00：00：00-23：59：59时间
        /// </summary>
        /// <returns>返回时间段</returns>
        public static DateTime[] GetDayTime()
        {
            DateTime times = DateTime.Now;  //当前时间  
            DateTime[] dateTimes = new DateTime[2];
            dateTimes[0] = new DateTime(times.Year, times.Month, times.Day, 0, 0, 0);
            dateTimes[1] = new DateTime(times.Year, times.Month, times.Day + 1);
            return dateTimes;
        }
        /// <summary>
        /// 返回一个周的时间段默认星期一到星期日为一周
        /// </summary>
        /// <returns>返回时间段</returns>
        public static DateTime[] GetWeekTime()
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));  //当前时间  
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
            DateTime endWeek = startWeek.AddDays(6);  //本周周日  
            return new DateTime[] { startWeek, endWeek.AddDays(1) };
        }

        /// <summary>
        /// 返回昨天的时间段
        /// </summary>
        /// <returns></returns>
        public static DateTime[] GetTodayTime()
        {
            DateTime dt = DateTime.Now;
            return new DateTime[] { new DateTime(dt.Year, dt.Month, dt.Day - 1, 0, 0, 0), new DateTime(dt.Year, dt.Month, dt.Day) };
        }

        /// <summary>
        /// 获取过去三个月时间
        /// </summary>
        /// <returns></returns>
        public static DateTime[] GetQuarterTime()
        {
            DateTime s, e;
            DateTime dt = DateTime.Now;

            s = new DateTime(dt.Year, dt.Month, dt.Day).AddMonths(-3);
            e = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(1);

            return new DateTime[] { s, e };
        }

        /// <summary>
        /// 获取过去半年时间
        /// </summary>
        /// <returns></returns>
        public static DateTime[] GetHalfYear()
        {
            DateTime s, e;
            DateTime dt = DateTime.Now;

            s = new DateTime(dt.Year, dt.Month, dt.Day).AddMonths(-6);
            e = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(1);

            return new DateTime[] { s, e };
        }

        /// <summary>
        /// 获取本月
        /// </summary>
        /// <returns></returns>
        public static DateTime[] GetOneYear()
        {
            DateTime s, e;
            DateTime dt = DateTime.Now;
            s = new DateTime(dt.Year, dt.Month, 1);
            e = dt.Date.AddDays(1);
            return new DateTime[] { s, e };
        }

        public static int WeekOfMonth(DateTime day, int WeekStart)
        {
            DateTime FirstofMonth;
            FirstofMonth = Convert.ToDateTime(day.Date.Year + "-" + day.Date.Month + "-" + 1);
            int i = (int)FirstofMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }
            if (WeekStart == 1)
            {
                return (day.Date.Day + i - 2) / 7 + 1;
            }
            if (WeekStart == 2)
            {
                return (day.Date.Day + i - 1) / 7;
            }
            return 0;
        }
    }
}
