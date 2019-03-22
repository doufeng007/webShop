using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLoan))]
    public class CreateCWGLoanInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 贷款金额
        /// </summary>  
        [Required, Money]
        public decimal Money { get; set; }

        /// <summary>
        /// 贷款用途及备注
        /// </summary>
        [Required,MaxLength(300)]
        public string Remark { get; set; }



        #endregion
    }
}