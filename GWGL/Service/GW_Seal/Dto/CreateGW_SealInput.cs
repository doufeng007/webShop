using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    [AutoMapTo(typeof(GW_Seal))]
    public class CreateGW_SealInput 
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(500,ErrorMessage = "名称长度必须小于500")]
        [Required(ErrorMessage = "必须填写名称")]
        public string Name { get; set; }

        /// <summary>
        /// KeepUser
        /// </summary>
        [Range(0, long.MaxValue,ErrorMessage="")]
        public long KeepUser { get; set; }

        /// <summary>
        /// AuditUser
        /// </summary>
        [MaxLength(100,ErrorMessage = "授权人长度必须小于100")]
        [Required(ErrorMessage = "必须填写授权人")]
        public string AuditUser { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        public GW_SealTypeEnmu SealType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_SealStatusEnmu Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
}