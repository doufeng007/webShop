using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Common;

namespace ZCYX.FRMSCore.Extensions
{
    public static class StringExtensions
    {
        public static bool GetStrContainsArray(this string str, string parm)
        {
            return !string.IsNullOrEmpty(str) && (str == parm ||
                                                  str.Contains($",{parm},") ||
                                                  str.IndexOf($"{parm},") == 0 ||
                                                  (str.LastIndexOf($",{parm}") + ($",{parm}").Length == str.Length && str.LastIndexOf($",{parm}") >= 0)
                                                  );
        }


        public static bool GetStrContainsArrayWithChar(this string str, string parm, string charStr = ",")
        {
            return !string.IsNullOrEmpty(str) && (str == parm ||
                                                  str.Contains($"{charStr}{parm}{charStr}") ||
                                                  str.IndexOf($"{parm}{charStr}") == 0 ||
                                                  (str.LastIndexOf($"{charStr}{parm}") + ($"{charStr}{parm}").Length == str.Length && str.LastIndexOf($"{charStr}{parm}") >= 0)
                                                  );
        }

        public static string GetMd5(this string str)
        {
            return Md5Helper.GetMd5(str);
        }


        public static bool GetFlowContainHideTask(this string str, int parm)
        {
            if (string.Compare(str.ToString(), "81427e46-80cc-4306-b931-b57973a699c8", true) == 0 ||
                   string.Compare(str.ToString(), "703A5047-017B-41F7-A558-D0AFAAD7AC32", true) == 0 ||
                   string.Compare(str.ToString(), "665548AE-A5FB-4EEE-89DE-A00B6875949E", true) == 0)
            {
                if (parm == 0 || parm == 1 || parm == -1)
                    return true;
            }
            else
            {
                if (parm == 0 || parm == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
