using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WeChat
{
    public class SendProjectCheckWxMessageInput
    {
        public Guid ProjectId { get; set; }
    }

    public class SendProjectCheckWxMessageOutput
    {
        public bool IsSuccefull { get; set; }


        public string RetMsg { get; set; }

    }
}
