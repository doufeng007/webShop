using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    /// <summary>
    /// 通配符类
    /// </summary>
    public class Wildcard
    {
        public static List<string> WildcardList = new List<string>() {
                    "$userid$",
                    "$username$",
                    "$deptid$",
                    "$deptname$",
                    "$unitid$",
                    "$unitname$",
                    "$account$",
                    "$querystring",
                    "$queryform",
                    "$datarow",
                    "$date"
                };
        ///// <summary>
        ///// 得到通配符的值
        ///// </summary>
        ///// <param name="sildcard"></param>
        ///// <returns></returns>
        //public static string GetWildcardValue(string wildcard, long userID)
        //{
        //    if (wildcard.IsNullOrEmpty())
        //    {
        //        return "";
        //    }
        //    string value = string.Empty;

        //    switch (wildcard.ToLower())
        //    {
        //        case "$userid$":
        //            value = userID.ToString();
        //            break;
        //        case "$username$":
        //            if (userID.IsGuid())
        //            {
        //                var user = new RoadFlow.Platform.Users().Get(userID.ToGuid());
        //                value = user == null ? "" : user.Name;
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentUserName;
        //            }
        //            break;
        //        case "$deptid$":
        //            if (userID.IsGuid())
        //            {
        //                var dept = new RoadFlow.Platform.Users().GetDeptByUserID(userID.ToGuid());
        //                value = dept == null ? "" : dept.ID.ToString();
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentDeptID.ToString();
        //            }
        //            break;
        //        case "$deptname$":
        //            if (userID.IsGuid())
        //            {
        //                var dept = new RoadFlow.Platform.Users().GetDeptByUserID(userID.ToGuid());
        //                value = dept == null ? "" : dept.Name;
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentDeptName.ToString();
        //            }
        //            break;
        //        case "$unitid$":
        //            if (userID.IsGuid())
        //            {
        //                var unit = new RoadFlow.Platform.Users().GetUnitByUserID(userID.ToGuid());
        //                value = unit == null ? "" : unit.ID.ToString();
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentUnitID.ToString();
        //            }
        //            break;
        //        case "$unitname$":
        //            if (userID.IsGuid())
        //            {
        //                var unit = new RoadFlow.Platform.Users().GetUnitByUserID(userID.ToGuid());
        //                value = unit == null ? "" : unit.Name;
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentUnitName.ToString();
        //            }
        //            break;
        //        case "$account$":
        //            if (userID.IsGuid())
        //            {
        //                var user = new RoadFlow.Platform.Users().Get(userID.ToGuid());
        //                value = user == null ? "" : user.Account;
        //            }
        //            else
        //            {
        //                value = RoadFlow.Platform.Users.CurrentUserAccount;
        //            }
        //            break;
        //    }
        //    return value;
        //}


        ///// <summary>
        ///// 过滤通配符
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string FilterWildcard(string str, string userID = "", object data = null)
        //{
        //    if (str.IsNullOrEmpty())
        //    {
        //        return "";
        //    }

        //    foreach (string s in WildcardList)
        //    {
        //        while (str.Contains(s, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            string value = string.Empty;
        //            switch (s)
        //            {
        //                case "$querystring":
        //                    if (str.Contains("$querystring", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        string qskey = str.Substring(str.IndexOf("$querystring&") + 13);
        //                        qskey = qskey.Substring(0, qskey.IndexOf("&$"));
        //                        value = System.Web.HttpContext.Current.Request.QueryString[qskey];
        //                        str = str.Replace1(s + "&" + qskey + "&$", value.IsNullOrEmpty() ? "" : value);
        //                    }
        //                    break;
        //                case "$queryform":
        //                    if (str.Contains("$queryform", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        string qfkey = str.Substring(str.IndexOf("$queryform&") + 11);
        //                        qfkey = qfkey.Substring(0, qfkey.IndexOf("&$"));
        //                        value = System.Web.HttpContext.Current.Request.Form[qfkey];
        //                        str = str.Replace1(s + "&" + qfkey + "&$", value.IsNullOrEmpty() ? "" : value);
        //                    }
        //                    break;
        //                case "$datarow":
        //                    if (str.Contains("$datarow", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        string drkey = str.Substring(str.IndexOf("$datarow&") + 9);
        //                        drkey = drkey.Substring(0, drkey.IndexOf("&$"));
        //                        value = "";
        //                        if (data != null && (data is System.Data.DataRow))
        //                        {
        //                            System.Data.DataRow dr = data as System.Data.DataRow;
        //                            value = dr[drkey].ToString();
        //                        }
        //                        str = str.Replace1(s + "&" + drkey + "&$", value.IsNullOrEmpty() ? "" : value);
        //                    }
        //                    break;
        //                case "$date"://替换日期格式
        //                    if (str.Contains("$date", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        string drkey = str.Substring(str.IndexOf("$date&") + 6);
        //                        drkey = drkey.Substring(0, drkey.IndexOf("&$"));
        //                        value = Utility.DateTimeNew.Now.ToString(drkey);
        //                        str = str.Replace1(s + "&" + drkey + "&$", value.IsNullOrEmpty() ? "" : value);
        //                    }
        //                    break;
        //                default:
        //                    value = GetWildcardValue(s, userID);
        //                    str = str.Replace1(s, value.IsNullOrEmpty() ? "" : value);
        //                    break;
        //            }
        //        }
        //    }
        //    return str;
        //}
    }
}
