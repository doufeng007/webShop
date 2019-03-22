using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace Train
{
    public class TrainExperienceListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训名称
        /// </summary>
        public string Title { get; set; }
        public string JoinUser { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

       public int SendCount { get; set; }
       public int NoSendCount { get; set; }

    }
}
