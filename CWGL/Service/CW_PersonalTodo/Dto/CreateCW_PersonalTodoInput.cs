using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapTo(typeof(CW_PersonalTodo))]
    public class CreateCW_PersonalTodoInput 
    {
        #region 表字段
        /// <summary>
        /// BusinessId
        /// </summary>
        [MaxLength(100,ErrorMessage = "业务编号长度必须小于100")]
        [Required(ErrorMessage = "必须填写业务编号")]
        public string BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CW_PersonalType BusinessType { get; set; }

        /// <summary>
        /// CWType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public RefundResultType CWType { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Amout_Pay { get; set; }


        public decimal Amout_Gather { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CW_PersonalToStatus Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [MaxLength(500,ErrorMessage = "备注长度必须小于500")]
        public string Remark { get; set; }

        /// <summary>
        /// FlowId
        /// </summary>
        public Guid? FlowId { get; set; }

        public string Title { get; set; }

        public long UserId { get; set; }

        #endregion
    }
}