using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLReimbursement))]
    public class CWGLReimbursementListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        public string UserName { get; set; }
        public string Note { get; set; }


        public string DepartmentName { get; set; }

        public long OrgId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
    }
}
