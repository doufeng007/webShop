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

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLWagePay))]
    public class CWGLWagePayOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 工资日期
        /// </summary>
        public DateTime WageDate { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime DoTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }


        


        //public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
