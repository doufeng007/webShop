using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    /// <summary>
    /// 员工外出获取列表参数  员工查看自己的  人力资源部查看全部 人力资源部可以按人 按部门 赛选；
    /// </summary>
    public class GetEmployeeGoOutListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        public long? CreateUserId { get; set; }

        public long? OrgId { get; set; }

        public List<int> Status { get; set; }

        public Guid FlowId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }

        public GetEmployeeGoOutListInput()
        {
            this.Status = new List<int>();
        }
    }
}
