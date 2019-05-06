using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WeChat
{
    /// <summary>
    /// 发送模板消息结果
    /// </summary>
    public class TemplateMessageResult : WxJsonResult
    {
        /// <summary>
        /// msgid
        /// </summary>
        public string msgid { get; set; }
    }
}
