using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(TrainUserExperience))]
    public class TrainUserExperienceListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// 领导批示
        /// </summary>
        public string Approval { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }

    public class TrainUserExperienceSumOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// 领导批示
        /// </summary>
        public string Approval { get; set; }


        public bool IsOver { get; set; }


        public DateTime? CreationTime { get; set; }

    }
    public class TrainUserExperienceOrganListOutputDto
    {
        public string OrganName { get; set; }
        public long Id { get; set; }
        public string LeaderName { get; set; }
        public long LeaderId { get; set; }

        /// <summary>
        /// 批示意见
        /// </summary>
        public string Approval { get; set; }
        public DateTime? ApprovalTime { get; set; }
        public List<long> Users { get; set; }
        public List<TrainUserExperienceListOutputDto> List { get; set; }


    }
}
