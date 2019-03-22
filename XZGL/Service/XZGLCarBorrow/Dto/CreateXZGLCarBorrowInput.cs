using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace XZGL
{
    [AutoMapTo(typeof(XZGLCarBorrow))]
    public class CreateXZGLCarBorrowInput : CreateWorkFlowInstance
    {
        #region 表字段

        /// <summary>
        /// 使用人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [MaxLength(50,ErrorMessage = "联系方式长度必须小于50")]
        public string Tel { get; set; }

        /// <summary>
        /// 用车开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 用车结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 用车类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CarType CarType { get; set; }

        /// <summary>
        /// 用车事由
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 单位车辆备注
        /// </summary>
        public string CompanyRemark { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 供应商联系方式
        /// </summary>
        [MaxLength(50,ErrorMessage = "供应商联系方式长度必须小于50")]
        public string SupplierTel { get; set; }

        /// <summary>
        /// 供应商备注
        /// </summary>
        public string SupplierRemark { get; set; }

        /// <summary>
        /// 安排用车备注
        /// </summary>
        public string CarRemark { get; set; }

        /// <summary>
        /// 用油量
        /// </summary>
        [MaxLength(20,ErrorMessage = "用油量长度必须小于20")]
        public string Consumption { get; set; }

        /// <summary>
        /// 用车归还备注
        /// </summary>
        public string CarReturnRemark { get; set; }

        /// <summary>
        /// 其他备注
        /// </summary>
        public string OtherRemark { get; set; }

        /// <summary>
        /// 个人用车备注
        /// </summary>
        public string UserCarRemark { get; set; }

        /// <summary>
        /// 是否单位车辆
        /// </summary>
        public bool IsCompanyCar { get; set; }

        /// <summary>
        /// 是否单位租车
        /// </summary>
        public bool IsCompanyRent { get; set; }

        /// <summary>
        /// 是否个人租车
        /// </summary>
        public bool IsUserRent { get; set; }


		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}