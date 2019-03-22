using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLTravelReimbursement))]
    public class CreateCWGLTravelReimbursementInput : CreateWorkFlowInstance
    {
        #region 表字段

        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        public decimal Money { get; set; }

        ///// <summary>
        ///// 报销处理结果
        ///// </summary>
        //public int ResultType { get; set; }

        ///// <summary>
        ///// 报销合计金额
        ///// </summary>
        //public decimal TotalMoney { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        public Guid? BorrowMoneyId { get; set; }

        /// <summary>
        /// 关联出差
        /// </summary>
        [Required]
        public Guid WorkoutId { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();


        public List<CreateOrUpdateCWGLTravelReimbursementDetailInput> DetailList { get; set; } = new List<CreateOrUpdateCWGLTravelReimbursementDetailInput>();


        #endregion
    }
}