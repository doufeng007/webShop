using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GWGL
{
    [AutoMapTo(typeof(Seal_Log))]
    public class CreateSeal_LogInput 
    {
        #region 表字段
        /// <summary>
        /// Seal_Id
        /// </summary>
        public Guid Seal_Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required(ErrorMessage = "必须填写Title")]
        public string Title { get; set; }

        /// <summary>
        /// Copies
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Copies { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }


		
        #endregion
    }
}