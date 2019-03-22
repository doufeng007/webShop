using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ZCYX.FRMSCore.Common
{
    public static class EnumerableHelper
    {
        public static List<dynamic> GetRange(List<DateTime> query, DateTime minTime, DateTime maxTime)
        {
            var result = new List<dynamic>();
            if (query.Any())
            {
                if ((maxTime - minTime).Days == 0)
                {
                    var index = "";
                    var getday = "";
                    for (; minTime.CompareTo(maxTime.AddHours(1)) < 0; minTime = minTime.AddHours(1))
                    {
                        if (index != minTime.ToString("yy.MM.dd"))
                        {
                            getday = "yy.MM.dd ";
                            index = minTime.ToString("yy.MM.dd");
                        }
                        dynamic obj = new ExpandoObject();
                        obj.Key = minTime.ToString(getday + "HH时");
                        getday = "";
                        obj.Value = query.Count(x => x.Hour == minTime.Hour && x.Day == minTime.Day);
                        result.Add(obj);
                    }
                }
                else if ((maxTime - minTime).Days <= 7)
                {
                    for (; minTime.Date.CompareTo(maxTime.Date.AddDays(1)) < 0; minTime = minTime.AddDays(1).Date)
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Key = minTime.Date.ToString("yyyy年MM月dd日");
                        obj.Value = query.Count(x => x.Date == minTime.Date);
                        result.Add(obj);
                    }
                }
                else if ((maxTime - minTime).Days <= 28)
                {
                    int dayOfWeek = Convert.ToInt32(minTime.DayOfWeek);
                    int daydiff = (-1) * dayOfWeek + 1;
                    var min = minTime.AddDays(daydiff).Date < Convert.ToDateTime(minTime.ToString("yyyy-MM-01"))
                        ? Convert.ToDateTime(minTime.ToString("yyyy-MM-01"))
                        : minTime.AddDays(daydiff).Date;
                    var diff = -1;
                    for (; min.CompareTo(maxTime.Date.AddDays(1)) < 0; min = min.AddDays(1))
                    {
                        if (diff != DateTimeHelper.WeekOfMonth(min, 1))
                        {
                            diff = DateTimeHelper.WeekOfMonth(min, 1);
                            dynamic obj = new ExpandoObject();
                            obj.Key = min.ToString("yyyy年MM月第") + DateTimeHelper.WeekOfMonth(min, 1) + "周";
                            dayOfWeek = Convert.ToInt32(min.DayOfWeek);
                            daydiff = (-1) * dayOfWeek + 1;
                            int dayadd = 7 - dayOfWeek;
                            var _min = min.AddDays(daydiff);
                            var _max = min.AddDays(dayadd).Date.AddDays(1);
                            obj.Value = query.Count(x => x.Date >= _min && x.Date < _max);
                            result.Add(obj);
                        }
                    }
                }
                else if ((maxTime - minTime).Days <= 365)
                {
                    var diff = -1;
                    for (; minTime.Date.CompareTo(maxTime.Date.AddDays(1)) < 0; minTime = minTime.AddDays(1).Date)
                    {
                        if (diff != minTime.Date.Month)
                        {
                            diff = minTime.Date.Month;
                            dynamic obj = new ExpandoObject();
                            obj.Key = minTime.ToString("yyyy年MM月");
                            var min = Convert.ToDateTime(minTime.ToString("yyyy-MM") + "-01");
                            var max =
                                Convert.ToDateTime(minTime.ToString("yyyy-MM") + "-01").AddMonths(1);
                            obj.Value = query.Count(x => x.Date >= min && x.Date < max);
                            result.Add(obj);
                        }
                    }
                }
                else
                {
                    var diff = -1;
                    for (; minTime.Date.CompareTo(maxTime.Date.AddDays(1)) < 0; minTime = minTime.AddDays(1).Date)
                    {
                        if (diff != minTime.Date.Year)
                        {
                            diff = minTime.Date.Year;
                            dynamic obj = new ExpandoObject();
                            obj.Key = minTime.ToString("yyyy年");
                            var min = Convert.ToDateTime(minTime.ToString("yyyy-01") + "-01");
                            var max =
                                Convert.ToDateTime(minTime.ToString("yyyy-01") + "-01").AddYears(1);
                            obj.Value = query.Count(x => x.Date >= min && x.Date < max);
                            result.Add(obj);
                        }
                    }
                }
            }
            return result;
        }
    }
}
