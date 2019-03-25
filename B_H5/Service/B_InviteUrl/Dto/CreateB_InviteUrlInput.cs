using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_InviteUrl))]
    public class CreateB_InviteUrlInput 
    {
        #region 表字段
        /// <summary>
        /// AgencyLevel
        /// </summary>
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// ValidityDataType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int ValidityDataType { get; set; }

        /// <summary>
        /// AvailableCount
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int AvailableCount { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [MaxLength(200,ErrorMessage = "Url长度必须小于200")]
        [Required(ErrorMessage = "必须填写Url")]
        public string Url { get; set; }


		
        #endregion
    }
}