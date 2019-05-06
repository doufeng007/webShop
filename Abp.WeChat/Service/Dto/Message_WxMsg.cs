using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WeChat
{
    public class Message_WxMsg
    {

        /// <summary>
        /// 推送人
        /// </summary>
        [DisplayName(@"推送人")]
        public Guid MId { get; set; }

        /// <summary>
        /// 接收经销商
        /// </summary>

        /// <summary>
        /// 推送标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 推送内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 接收人openid
        /// </summary>
        public string ReceiveOpenId { get; set; }

        /// <summary>
        /// 微信回传消息编号
        /// </summary>
        public string MsgID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enum_WxMsgStatus Status { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 模块内容
        /// </summary>
        public string TempContent { get; set; }
        
    }
}
