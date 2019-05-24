using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_ManagerPayAccount))]
    public class CreateB_ManagerPayAccountInput 
    {
        #region 表字段
        /// <summary>
        /// Type
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public PayAccountType Type { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        [MaxLength(100,ErrorMessage = "Account长度必须小于100")]
        [Required(ErrorMessage = "必须填写Account")]
        public string Account { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [MaxLength(50, ErrorMessage = "BankName长度必须小于50")]

        public string BankName { get; set; } = "";

        /// <summary>
        /// BankUserName
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankUserName长度必须小于50")]
        
        public string BankUserName { get; set; } = "";

        /// <summary>
        /// WxName
        /// </summary>
        [MaxLength(50,ErrorMessage = "WxName长度必须小于50")]
        [Required(ErrorMessage = "必须填写WxName")]
        public string WxName { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [MaxLength(200,ErrorMessage = "Remark长度必须小于200")]
        public string Remark { get; set; }


		
        #endregion
    }
}