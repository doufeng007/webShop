using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace CWGL
{
    [AutoMapTo(typeof(FACertificateDetail))]
    public class CreateFACertificateDetailInput
    {
        #region 表字段

        public Guid? Id { get; set; }
        /// <summary>
        /// 会计科目id
        /// </summary>
        public Guid AccountingCourseId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 借贷类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }



        #endregion
    }
}