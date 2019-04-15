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
        /// 姓名
        /// </summary>
        public string Name { get; set; }


        ///// <summary>
        /////代理级别 
        ///// </summary>
        //public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// 代理编码
        /// </summary>
        [MaxLength(100, ErrorMessage = "AgenCyCode长度必须小于100")]
        [Required(ErrorMessage = "必须填写AgenCyCode")]
        public string AgenCyCode { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [MaxLength(100, ErrorMessage = "Provinces长度必须小于100")]
        public string Provinces { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        [MaxLength(100, ErrorMessage = "County长度必须小于100")]
        public string County { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [MaxLength(100, ErrorMessage = "City长度必须小于100")]
        public string City { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [MaxLength(200, ErrorMessage = "Address长度必须小于200")]
        public string Address { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public B_AgencyTypeEnum Type { get; set; }

        /// <summary>
        /// 签约日期
        /// </summary>
        public DateTime SignData { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        public string Agreement { get; set; }


        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? P_Id { get; set; }



        /// <summary>
        /// 电话 作为账号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WxId { get; set; }


        #endregion
    }
}