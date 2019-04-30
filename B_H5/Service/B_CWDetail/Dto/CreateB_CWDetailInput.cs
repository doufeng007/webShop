using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_CWDetail))]
    public class CreateB_CWDetailInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// RelationUserId
        /// </summary>
        public long? RelationUserId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CWDetailTypeEnum Type { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CWDetailBusinessTypeEnum BusinessType { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Number { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }


		
        #endregion
    }
}