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
    [AutoMapFrom(typeof(Train))]
    public class TrainListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训名称
        /// </summary>
        public string Title { get; set; }
        public Guid Type { get; set; }

        /// <summary>
        /// 培训类别
        /// </summary>
        public string TypeName { get; set; }
        public string LecturerUser { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 培训开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 培训结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        public bool IsOver { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }

    }
}
