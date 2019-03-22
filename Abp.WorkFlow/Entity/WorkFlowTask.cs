using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Abp.WorkFlow
{
    [Serializable]
    [Table("WorkFlowTask")]
    public class WorkFlowTask : IEntity<Guid>
    {


        public WorkFlowTask DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as WorkFlowTask;
            }
        }

        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 上一任务ID
        /// </summary>
        [DisplayName("PrevID")]
        public Guid PrevID { get; set; }

        /// <summary>
        /// 上一步骤ID
        /// </summary>
        [DisplayName("PrevStepID")]
        public Guid PrevStepID { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        [DisplayName("FlowID")]
        public Guid FlowID { get; set; }

        /// <summary>
        /// 步骤ID
        /// </summary>
        [DisplayName("StepID")]
        public Guid StepID { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        [DisplayName("StepName")]
        public string StepName { get; set; }

        /// <summary>
        /// 实例ID
        /// </summary>
        [DisplayName("InstanceID")]
        public string InstanceID { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        [DisplayName("GroupID")]
        public Guid GroupID { get; set; }

        /// <summary>
        /// 任务类型 0正常 1指派 2委托 3转交 4退回 5抄送  6 子任务  7加签  8意见征询
        /// </summary>
        [DisplayName("Type")]
        public int Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        [DisplayName("SenderID")]
        public long SenderID { get; set; }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        [DisplayName("SenderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [DisplayName("SenderTime")]
        public DateTime SenderTime { get; set; }

        /// <summary>
        /// 接收人员ID
        /// </summary>
        [DisplayName("ReceiveID")]
        public long ReceiveID { get; set; }

        /// <summary>
        /// 接收人员姓名
        /// </summary>
        [DisplayName("ReceiveName")]
        public string ReceiveName { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        [DisplayName("ReceiveTime")]
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 打开时间
        /// </summary>
        [DisplayName("OpenTime")]
        public DateTime? OpenTime { get; set; }

        /// <summary>
        /// 规定完成时间
        /// </summary>
        [DisplayName("CompletedTime")]
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        [DisplayName("CompletedTime1")]
        public DateTime? CompletedTime1 { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        [DisplayName("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// 是否签章 0未签 1已签
        /// </summary>
        [DisplayName("IsSign")]
        public int? IsSign { get; set; }

        /// <summary>
        /// 状态 -1等待中的任务 0待处理 1打开 2完成 3退回 4他人已处理 5他人已退回 6终止  7他人已终止 8退回审核 9申请停滞    10 被管理员作废 11 删除 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }

        /// <summary>
        /// 其它说明
        /// </summary>
        [DisplayName("Note")]
        public string Note { get; set; }

        /// <summary>
        /// 处理顺序号
        /// </summary>
        [DisplayName("Sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 子流程实例分组ID
        /// </summary>
        [DisplayName("SubFlowGroupID")]
        public string SubFlowGroupID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherType")]
        public int? OtherType { get; set; }

        /// <summary>
        /// 相关附件
        /// </summary>
        [DisplayName("Files")]
        public string Files { get; set; }

        public int? TodoType { get; set; }

        /// <summary>
        /// 流程退回会引起Deepth+1 主要用于获取最新的意见；
        /// </summary>
        public int Deepth { get; set; }


        public int VersionNum { get; set; }

        /// <summary>
        /// 意见征询分组
        /// </summary>
        [DisplayName("InquiryGroupID")]
        public Guid? InquiryGroupID { get; set; }

        /// <summary>
        /// 意见征询
        /// </summary>
        [DisplayName("Inquiry")]
        public string Inquiry { get; set; }
        public Guid? SignFileId { get; set; }

        /// <summary>
        /// 委托任务
        /// </summary>
        [DisplayName("RelationId")]
        public Guid? RelationId { get; set; }

    }


    public class WorkTasksEqualityComparer : IEqualityComparer<WorkFlowTask>
    {
        public bool Equals(WorkFlowTask task1, WorkFlowTask task2)
        {
            return task1 == null || task2 == null || task1.Id == task2.Id;
        }
        public int GetHashCode(WorkFlowTask task)
        {
            return task.ToString().GetHashCode();
        }
    }
}