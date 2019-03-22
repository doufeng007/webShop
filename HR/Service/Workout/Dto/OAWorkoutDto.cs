using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    [AutoMap(typeof(OAWorkout))]
    public class OAWorkoutInputDto : CreateWorkFlowInstance
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
        /// 外出原因
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

        public string Destination { get; set; }

        /// <summary>
        /// 出发地点
        /// </summary>
        public string FromPosition { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public Guid TranType { get; set; }
        public long? RelationUserId { get; set; }
        public bool? IsCar { get; set; }

        public bool IsUpdateForChange { get; set; }

    }
    [AutoMap(typeof(OAWorkout))]
    public class OAWorkoutDto : WorkFlowTaskCommentResult
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
        /// 外出原因
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

        public string UserId_Name { get; set; }


        public string Destination { get; set; }

        /// <summary>
        /// 出发地点
        /// </summary>
        public string FromPosition { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public Guid TranType { get; set; }


        public long? RelationUserId { get; set; }
        public bool? IsCar { get; set; }
        public string  RelationUserName { get; set; }
    }


    public class GetOAWorkoutListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public string Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetOAWorkoutListByCarInput {
        public long? UserId { get; set; }
        public bool IsOld { get; set; } = false;
        public List<Guid> List { get; set; } = new List<Guid>();
    }
    public class OAWorkoutListByCarDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


    }

    [AutoMap(typeof(OAWorkout))]
    public class OAWorkoutListDto : BusinessWorkFlowListOutput
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

        public string Destination { get; set; }

        public long OrgId { get; set; }


        public string FromPosition { get; set; }


        public Guid TranType { get; set; }

        public long? RelationUserId { get; set; }
        public bool? IsCar { get; set; }
    }
}
