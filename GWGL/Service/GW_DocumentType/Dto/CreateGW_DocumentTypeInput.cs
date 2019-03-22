using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    [AutoMapTo(typeof(GW_DocumentType))]
    public class CreateGW_DocumentTypeInput 
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(500,ErrorMessage = "名称长度必须小于500")]
        [Required(ErrorMessage = "必须填写名称")]
        public string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public GW_DocumentTypeEnmu Type { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_EmployeesSignStatusEnmu Status { get; set; }


		
        #endregion
    }
}