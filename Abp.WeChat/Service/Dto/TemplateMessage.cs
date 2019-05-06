using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Abp.WeChat
{
    /// <summary>
    /// 公众号模板消息
    /// </summary>
    public class TemplateMessage
    {
        public TemplateMessage()
        {
            topcolor = "#FF0000";
        }

        /// <summary>
        /// 接收者微信OpenId
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 跳转url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 顶部颜色
        /// </summary>
        public string topcolor { get; set; }

        /// <summary>
        /// 具体模板数据
        /// </summary>
        public object data { get; set; }
    }

    public class TemplateDataItem
    {
        /// <summary>
        /// 项目值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 16进制颜色代码，如：#FF0000
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">value</param>
        /// <param name="c">color</param>
        public TemplateDataItem(string v, string c = "#173177")
        {
            value = v;
            color = c;
        }
    }

    /// <summary>
    /// 消息模板
    /// </summary>
    public class MessageTemplate
    {

        /// <summary>
        /// 经销商平台通知
        /// </summary>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <param name="keyword3"></param>
        /// <param name="keyword4"></param>
        /// <param name="keyword5"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static object NoticeDealer(string first, Dictionary<string, string> keyword, string remark)
        {
            StringBuilder _sb=new StringBuilder();
            _sb.Append("{\"first\":{\"value\":\""+ first + "\",\"color\":\"#000000\"},");
            keyword.ToList().ForEach(x =>
            {
                _sb.Append("\"" + x.Key + "\":{\"value\":\"" + x.Value +
                             "\",\"color\":\"#000000\"},");
            });
            _sb.Append("\"remark\":{\"value\":\""+ remark + "\",\"color\":\"#000000\"}");
            _sb.Append("}");
            return JsonConvert.DeserializeObject(_sb.ToString());
        }
    }
}
