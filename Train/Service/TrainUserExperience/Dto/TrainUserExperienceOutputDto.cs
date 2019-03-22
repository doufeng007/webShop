using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(TrainUserExperience))]
    public class TrainUserExperienceOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

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


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();



    }
}
