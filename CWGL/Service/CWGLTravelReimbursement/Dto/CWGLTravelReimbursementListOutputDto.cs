using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLTravelReimbursement))]
    public class CWGLTravelReimbursementListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 报销人
        /// </summary>
        public string UserId_Name { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string OrgId_Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}
