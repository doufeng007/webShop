using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_AgencyLevel))]
    public class CreateB_AgencyLevelInput 
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(500,ErrorMessage = "Name长度必须小于500")]
        [Required(ErrorMessage = "必须填写Name")]
        public string Name { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Level { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }


		
        #endregion
    }
}