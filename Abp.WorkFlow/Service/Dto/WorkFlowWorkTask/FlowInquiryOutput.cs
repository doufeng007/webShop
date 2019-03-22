using Abp.File;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Abp.WorkFlow
{
    public class FlowInquiryOutput
    {
        public string Inquiry { get; set; }
        public DateTime ReceiveTime { get; set; }
        public string SenderName { get; set; }


        public List<FlowInquiryTask> Tasks { get; set; }
    }
    public class FlowInquiryTask
    {
        public Guid Id { get; set; }

        public string Comment { get; set; }
        public string ReceiveName { get; set; }
        public DateTime? CompletedTime1 { get; set; }


        public List<GetAbpFilesOutput> Files { get; set; } = new List<GetAbpFilesOutput>();
    }
    public class GetFlowInquiryInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 流程id
        /// </summary>
        public Guid FlowId { get; set; }

        ///// <summary>
        ///// 实例id
        ///// </summary>
        //public string InstanceId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public Guid TaskId { get; set; }

        ///// <summary>
        ///// 步骤id
        ///// </summary>
        //public Guid StepId { get; set; }
    }


    public class FlowInquiryInput
    {
        /// <summary>
        /// 流程id
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 实例id
        /// </summary>
        public string InstanceId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public Guid StepId { get; set; }

        [Required(ErrorMessage = "请输入意见咨询内容。")]
        public string Inquiry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid GroupId { get; set; }

        public string UserIds { get; set; }

    }
}