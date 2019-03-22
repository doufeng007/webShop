using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace XZGL
{
    [AutoMapFrom(typeof(XZGLCarBorrow))]
    public class XZGLCarBorrowListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        public long UserId { get; set; }
        public string CreatorUserName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
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
        public CarType CarType { get; set; }
        public string CarTypeName { get; set; }

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


    }
}
