using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAWorkon))]
    public class OAWorkonInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Hours { get; set; }
        /// <summary>
        /// 加班原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }

        public bool IsUpdateForChange { get; set; }

    }
    [AutoMap(typeof(OAWorkon))]
    public class OAWorkonDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int? Hours { get; set; }
        /// <summary>
        /// 加班原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }


        public string DepartmentName { get; set; }

        public string Post_Name { get; set; }
        public string PostIds { get; set; }
        public string UserId_Name { get; set; }

    }
    [AutoMap(typeof(OAWorkon))]
    public class OAWorkonListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Hours { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }

        public string OrgName { get; set; }


        public string PostName { get; set; }

        public DateTime CreationTime { get; set; }

        public string DepartmentName { get; set; }

        public string Reason { get; set; }


        public long UserId
        {
            get; set;
        }
        public string UserId_Name { get; set; }


        public long OrgId { get; set; }

    }
}
