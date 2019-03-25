using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_MyAddress))]
    public class CreateB_MyAddressInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        [MaxLength(100,ErrorMessage = "Provinces长度必须小于100")]
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        [MaxLength(100,ErrorMessage = "County长度必须小于100")]
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [MaxLength(100,ErrorMessage = "City长度必须小于100")]
        public string City { get; set; }

        /// <summary>
        /// Addres
        /// </summary>
        [MaxLength(500,ErrorMessage = "Addres长度必须小于500")]
        [Required(ErrorMessage = "必须填写Addres")]
        public string Addres { get; set; }

        /// <summary>
        /// Consignee
        /// </summary>
        [MaxLength(100,ErrorMessage = "Consignee长度必须小于100")]
        [Required(ErrorMessage = "必须填写Consignee")]
        public string Consignee { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        [MaxLength(50,ErrorMessage = "Tel长度必须小于50")]
        [Required(ErrorMessage = "必须填写Tel")]
        public string Tel { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }


		
        #endregion
    }
}