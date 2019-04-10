using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Agency))]
    public class CreateB_AgencyInput
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// AgencyLevel
        /// </summary>
        public int AgencyLevel { get; set; }


        public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// AgenCyCode
        /// </summary>
        [MaxLength(100, ErrorMessage = "AgenCyCode长度必须小于100")]
        [Required(ErrorMessage = "必须填写AgenCyCode")]
        public string AgenCyCode { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        [MaxLength(100, ErrorMessage = "Provinces长度必须小于100")]
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        [MaxLength(100, ErrorMessage = "County长度必须小于100")]
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [MaxLength(100, ErrorMessage = "City长度必须小于100")]
        public string City { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [MaxLength(200, ErrorMessage = "Address长度必须小于200")]
        public string Address { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public B_AgencyTypeEnum Type { get; set; }

        /// <summary>
        /// SignData
        /// </summary>
        public DateTime SignData { get; set; }

        /// <summary>
        /// Agreement
        /// </summary>
        public string Agreement { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? P_Id { get; set; }


        #endregion
    }
}