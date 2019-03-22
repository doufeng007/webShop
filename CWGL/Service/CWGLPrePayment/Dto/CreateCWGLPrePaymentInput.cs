using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLPrePayment))]
    public class CreateCWGLPrePaymentInput 
    {
        #region 表字段
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 事由说明
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Cause { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        [Required]
        [Money]
        public decimal Money { get; set; }


		
        #endregion
    }
}