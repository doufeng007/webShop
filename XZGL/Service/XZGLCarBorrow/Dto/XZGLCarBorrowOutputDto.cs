using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using System.ComponentModel;

namespace XZGL
{
    [AutoMapFrom(typeof(XZGLCarBorrow))]
    public class XZGLCarBorrowOutputDto : WorkFlowTaskCommentResult
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
        public string UserName { get; set; }
        public long? CreatorUserId { get; set; }
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
		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        public List<XZGLCarUserOutput> CarUser { get; set; } = new List<XZGLCarUserOutput>();
        public List<XZGLCarInfoOutput> CarInfo { get; set; } = new List<XZGLCarInfoOutput>();
        public XZGLCarUserInfo CarUserInfo { get; set; }
        public List<Guid> RelationWorkOut { get; set; } = new List<Guid>();
    }

    public class XZGLWorkOutCarOutputDto {
        public Guid Id { get; set; }
        public Guid CarBorrowId { get; set; }
        public string UserName { get; set; }
        public string CarTypeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    [AutoMapFrom(typeof(XZGLCarBorrow))]
    public class XZGLCarBorrowCarInfoOutputDto : WorkFlowTaskCommentResult
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
        public string UserName { get; set; }
        public long? CreatorUserId { get; set; }
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
        public XZGLCarInfoOutput CarInfo { get; set; }
    }

    public class XZGLCarInfoOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 车辆编号
        /// </summary>
        [DisplayName(@"车辆编号")]
        public Guid CarId { get; set; }
        /// <summary>
        /// 司机
        /// </summary>
        [DisplayName(@"司机")]
        public long UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        [DisplayName(@"品牌型号")]
        public string CarType { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        [DisplayName(@"座位数")]
        public string SeatNum { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [DisplayName(@"排量")]
        public string Amount { get; set; }

    }
    public class XZGLCarUserOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        [DisplayName(@"平台名称")]
        public string Name { get; set; }

        /// <summary>
        /// 平台地址
        /// </summary>
        [DisplayName(@"平台地址")]
        public string Address { get; set; }

        /// <summary>
        /// 平台联系方式
        /// </summary>
        [DisplayName(@"平台联系方式")]
        public string Tel { get; set; }

        /// <summary>
        /// 租车费用
        /// </summary>
        [DisplayName(@"租车费用")]
        public decimal CarMoney { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DisplayName(@"订单编号")]
        public string OrderNum { get; set; }

        /// <summary>
        /// 车辆品牌型号
        /// </summary>
        [DisplayName(@"车辆品牌型号")]
        public string CarType { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        public string CarNum { get; set; }
    }
}
