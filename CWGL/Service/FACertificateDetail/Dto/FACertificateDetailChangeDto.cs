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
using ZCYX.FRMSCore;

namespace CWGL
{
    public class FACertificateDetailChangeDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [LogColumn("主键", IsLog = false)]
        public Guid? Id { get; set; }


        [LogColumn("会计科目名称", IsNameField = true)]
        public string AccountingCourseName { get; set; }

        /// <summary>
        /// 借贷类型
        /// </summary>
        [LogColumn("借贷类型")]
        public string BusinessType_Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [LogColumn("金额")]
        public decimal Amount { get; set; }

		
    }
}
