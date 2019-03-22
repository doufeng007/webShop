using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GWGL
{
    [AutoMapTo(typeof(GW_FWZHRole))]
    public class CreateGW_FWZHRoleInput 
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(1000,ErrorMessage = "发文机关长度必须小于1000")]
        [Required(ErrorMessage = "必须填写发文机关")]
        public string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [MaxLength(1000,ErrorMessage = "发文机关代字长度必须小于1000")]
        [Required(ErrorMessage = "必须填写发文机关代字")]
        public string Code { get; set; }

        /// <summary>
        /// StartIndex
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int StartIndex { get; set; }

        /// <summary>
        /// AutoCoding
        /// </summary>
        public bool AutoCoding { get; set; }


		
        #endregion
    }
}