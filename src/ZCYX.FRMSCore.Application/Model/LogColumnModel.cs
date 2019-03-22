using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;

namespace ZCYX.FRMSCore
{
    public class LogColumnModel
    {
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }


        public int? ChangeType { get; set; }

        public new string ToString()
        {
            var ret = $"{FieldName}: ";
            if (this.ChangeType.HasValue)
            {
                if (this.ChangeType == 1)
                {
                    ret = ret + $"新增:{NewValue}";
                }
                else
                {
                    ret = ret + $"删除:{OldValue}";
                }
            }
            else
            {
                if (OldValue.IsNullOrWhiteSpace())
                    ret = ret + "无";
                else
                {
                    ret = ret + OldValue;
                }
                ret = ret + " 改变为: ";
                if (NewValue.IsNullOrWhiteSpace())
                    ret = ret + "无";
                else
                {
                    ret = ret + NewValue;
                }
            }


            return ret;
        }
    }


    public class ChangeLog
    {
        public string UserName { get; set; }

        public DateTime ChangeTime { get; set; }



        public List<LogColumnModel> ContentModel { get; set; }


        public ChangeLog()
        {
            ContentModel = new List<LogColumnModel>();
        }


    }

   
}
