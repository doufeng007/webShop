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
    [AutoMapFrom(typeof(FACertificateDetail))]
    public class FACertificateDetailOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 记账凭证主表id
        /// </summary>
        public Guid MainId { get; set; }

        /// <summary>
        /// 会计科目id
        /// </summary>
        public Guid AccountingCourseId { get; set; }

        /// <summary>
        /// 借贷类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
