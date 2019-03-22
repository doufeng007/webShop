using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZCYX.FRMSCore.Application
{
    [Table("NoticeTexts")]
    public class NoticeTexts : FullAuditedEntity<Guid>
    {

        /// <summary>
        /// 发送人ID
        /// </summary>
        [DisplayName("SendUserId")]
        public long SendUserId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 消息主体内容
        /// </summary>
        [DisplayName("MsgConent")]
        public string MsgConent { get; set; }

        /// <summary>
        /// 类型
        /// 1 事务通知 2 通知公告 3 公司新闻
        /// </summary>
        [DisplayName("NoticeType")]
        public int NoticeType { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [DisplayName("ExpireTime")]
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [DisplayName("LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 执行js方法
        /// </summary>
        [DisplayName("JsCommandText")]
        public string JsCommandText { get; set; }

        /// <summary>
        /// 要通知的用户逗号分隔
        /// </summary>
        public string NoticeUserIds { get; set; }

        /// <summary>
        /// 要通知的组
        /// </summary>
        public string NoticeGroupIds { get; set; }
        /// <summary>
        /// 要通知的部门
        /// </summary>
        public string NoticeDepartmentIds { get; set; }


        public string NoticeAllUserIds { get; set; }

    }
}