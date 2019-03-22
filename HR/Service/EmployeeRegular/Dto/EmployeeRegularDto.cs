using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{


    public class GetEmployeeRegularDtoInput : GetWorkFlowTaskCommentInput
    {
        public Guid Id { get; set; }
    }

    [AutoMapFrom(typeof(EmployeeRegular))]
    public class EmployeeRegularDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }


        public Guid EmployeeId { get; set; }


        public long UserId { get; set; }


        public string UserName { get; set; }


        public DateTime ApplyDate { get; set; }


        public bool IsAdvanced { get; set; }

        /// <summary>
        /// 提前申请的工作业绩补充
        /// </summary>
        public string WorkSummary { get; set; }


        public DateTime StrialBeginTime { get; set; }

        public DateTime StrialEndTime { get; set; }


        public string Remark { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; }

        public EmployeeRegularDto()
        {
            this.Files = new List<GetAbpFilesOutput>();
        }
    }
}
