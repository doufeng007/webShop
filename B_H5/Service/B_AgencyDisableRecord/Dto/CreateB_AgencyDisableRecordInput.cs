using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_AgencyDisableRecord))]
    public class CreateB_AgencyDisableRecordInput 
    {
        #region 表字段
        /// <summary>
        /// AgencyId
        /// </summary>
        public Guid AgencyId { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [MaxLength(500,ErrorMessage = "Reason长度必须小于500")]
        [Required(ErrorMessage = "必须填写Reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [MaxLength(500,ErrorMessage = "Remark长度必须小于500")]
        [Required(ErrorMessage = "必须填写Remark")]
        public string Remark { get; set; }


		
        #endregion
    }
}