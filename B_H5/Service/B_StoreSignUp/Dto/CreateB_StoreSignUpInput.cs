using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_StoreSignUp))]
    public class CreateB_StoreSignUpInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        [MaxLength(100,ErrorMessage = "Provinces长度必须小于100")]
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        [MaxLength(100,ErrorMessage = "County长度必须小于100")]
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [MaxLength(100,ErrorMessage = "City长度必须小于100")]
        public string City { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [MaxLength(500,ErrorMessage = "Address长度必须小于500")]
        public string Address { get; set; }

        /// <summary>
        /// BankNumber
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankNumber长度必须小于50")]
        public string BankNumber { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankUserName长度必须小于50")]
        public string BankUserName { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [MaxLength(100,ErrorMessage = "BankName长度必须小于100")]
        public string BankName { get; set; }

        /// <summary>
        /// OpenDate
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// StorArea
        /// </summary>
        [MaxLength(50,ErrorMessage = "StorArea长度必须小于50")]
        public string StorArea { get; set; }

        /// <summary>
        /// Goods
        /// </summary>
        public string Goods { get; set; }


		
        #endregion
    }
}