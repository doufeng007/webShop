using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_TrialProduct))]
    public class CreateB_TrialProductInput 
    {
        #region 表字段
        /// <summary>
        /// Title
        /// </summary>
        [MaxLength(500,ErrorMessage = "Title长度必须小于500")]
        [Required(ErrorMessage = "必须填写Title")]
        public string Title { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }


		
        #endregion
    }
}