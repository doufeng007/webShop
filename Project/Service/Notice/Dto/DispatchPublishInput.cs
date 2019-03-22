using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Project
{
    public class DispatchPublishInput
    {
        public CreateOrUpdateProjectBaseInput BaseOutput { get; set; }
        public Guid? Id { get; set; }
        /// <summary>
        /// 阶段ID
        /// </summary>
        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }


        public string SingleProjectName { get; set; }

        public string SingleProjectCode { get; set; }



        public int AppraisalTypeId { get; set; }

        /// <summary>
        /// 评审负责人
        /// </summary>
        public string ProjectLeader { get; set; }

        /// <summary>
        /// 评审人员
        /// </summary>
        public string ProjectReviewer { get; set; }

        /// <summary>
        /// 来文字号
        /// </summary>
        public string DispatchCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 审减原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 附加说明
        /// </summary>
        public string Additional { get; set; }

        /// <summary>
        /// 项目承办单编号
        /// </summary>
        public string ProjectUndertakeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 建设单位
        /// </summary>
        public string SendUnitName { get; set; }


        public decimal? AuditAmount { get; set; }

    }

    [AutoMapFrom(typeof(DispatchMessage))]
    public class DispatchPublishOutput: DispatchPublishInput
    {

    }
}