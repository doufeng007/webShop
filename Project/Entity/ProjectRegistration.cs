using Abp.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ProjectRegistration")]
    public class ProjectRegistration : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public Guid Id { get; set; }


        /// <summary>
        /// 工作记录ID
        /// </summary>
        [DisplayName("TaskId")]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("SendUserId")]
        public long SendUserId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 事项编号
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 事项名称
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 登记时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [DisplayName("WorkTime")]
        public DateTime WorkTime { get; set; }

        /// <summary>
        /// 登记类型
        /// 1工作联系 2意见征询
        /// </summary>
        [DisplayName("Type")]
        public int Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName("Content")]
        public string Content { get; set; }

        /// <summary>
        /// 收取人用户ID
        /// </summary>
        [DisplayName("RecieveUserId")]
        public long RecieveUserId { get; set; }

        /// <summary>
        /// 收取人是否查看
        /// </summary>
        [DisplayName("IsRead")]
        public bool IsRead { get; set; }

        /// <summary>
        /// 是否已发送邮件
        /// </summary>
        [DisplayName("IsSendEmail")]
        public bool IsSendEmail { get; set; }

        /// <summary>
        /// 是否已短信通知
        /// </summary>
        [DisplayName("IsSendSms")]
        public bool IsSendSms { get; set; }

        /// <summary>
        /// 是否已微信通知
        /// </summary>
        [DisplayName("IsSendWx")]
        public bool IsSendWx { get; set; }

        /// <summary>
        /// 是否进入发文
        /// </summary>
        [DisplayName("IsSendDispatch")]
        public bool IsSendDispatch { get; set; }

        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }


        public int Status { get; set; }
        /// <summary>
        /// 是否汇总
        /// </summary>
        public bool IsSummary { get; set; }
        /// <summary>
        /// 汇总编号
        /// </summary>
        public Guid? RegistrationId { get; set; }
        /// <summary>
        /// 负责人编号
        /// </summary>
        public long? PersonOnCharge { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public PersonOnChargeTypeEnum PersonOnChargeType { get; set; }



    }
}