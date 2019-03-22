using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace XZGL
{
	[AutoMapFrom(typeof(XZGLCarBorrow))]
    public class XZGLCarBorrowLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        [LogColumn(@"使用人", IsLog = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [LogColumn(@"联系方式", IsLog = true)]
        public string Tel { get; set; }

        /// <summary>
        /// 用车开始时间
        /// </summary>
        [LogColumn(@"用车开始时间", IsLog = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 用车结束时间
        /// </summary>
        [LogColumn(@"用车结束时间", IsLog = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 用车类型
        /// </summary>
        [LogColumn(@"用车类型", IsLog = true)]
        public string CarTypeName { get; set; }

        /// <summary>
        /// 用车事由
        /// </summary>
        [LogColumn(@"用车事由", IsLog = true)]
        public string Note { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [LogColumn(@"备注", IsLog = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 单位车辆备注
        /// </summary>
        [LogColumn(@"单位车辆备注", IsLog = true)]
        public string CompanyRemark { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        [LogColumn(@"供应商编号", IsLog = true)]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 供应商联系方式
        /// </summary>
        [LogColumn(@"供应商联系方式", IsLog = true)]
        public string SupplierTel { get; set; }

        /// <summary>
        /// 供应商备注
        /// </summary>
        [LogColumn(@"供应商备注", IsLog = true)]
        public string SupplierRemark { get; set; }

        /// <summary>
        /// 安排用车备注
        /// </summary>
        [LogColumn(@"安排用车备注", IsLog = true)]
        public string CarRemark { get; set; }

        /// <summary>
        /// 用油量
        /// </summary>
        [LogColumn(@"用油量", IsLog = true)]
        public string Consumption { get; set; }

        /// <summary>
        /// 用车归还备注
        /// </summary>
        [LogColumn(@"用车归还备注", IsLog = true)]
        public string CarReturnRemark { get; set; }

        /// <summary>
        /// 其他备注
        /// </summary>
        [LogColumn(@"其他备注", IsLog = true)]
        public string OtherRemark { get; set; }

        /// <summary>
        /// 个人用车备注
        /// </summary>
        [LogColumn(@"个人用车备注", IsLog = true)]
        public string UserCarRemark { get; set; }

        /// <summary>
        /// 是否单位车辆
        /// </summary>
        [LogColumn(@"是否单位车辆", IsLog = true)]
        public bool IsCompanyCar { get; set; }

        /// <summary>
        /// 是否单位租车
        /// </summary>
        [LogColumn(@"是否单位租车", IsLog = true)]
        public bool IsCompanyRent { get; set; }

        /// <summary>
        /// 是否个人租车
        /// </summary>
        [LogColumn(@"是否个人租车", IsLog = true)]
        public bool IsUserRent { get; set; }

        /// <summary>
        /// 个人用车
        /// </summary>
        [LogColumn(@"个人用车", IsLog = true)]
        public List<XZGLCarUserLogDto> CarUser { get; set; } = new List<XZGLCarUserLogDto>();

        /// <summary>
        /// 单位车辆
        /// </summary>
        [LogColumn(@"单位车辆", IsLog = true)]
        public List<XZGLCarInfoLogDto> CarInfo { get; set; } = new List<XZGLCarInfoLogDto>();

        /// <summary>
        /// 出差
        /// </summary>
        [LogColumn(@"出差", IsLog = true)]
        public List<XZGLWorkoutListByCarDto> Workouts { get; set; } = new List<XZGLWorkoutListByCarDto>();
        /// <summary>
        /// 私车公用
        /// </summary>
        [LogColumn(@"私车公用", IsLog = true)]
        public XZGLCarUserInfoLogDto CarUserInfo { get; set; }


        [LogColumn("电子资料", IsLog = true)]
        public List<AbpFileChangeDto> Files { get; set; } = new List<AbpFileChangeDto>();
        #endregion
    }
    public class XZGLWorkoutListByCarDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid? Id { get; set; }
        [LogColumn("出差地点", IsLog = true)]
        public string Address { get; set; }
        [LogColumn("姓名", IsNameField = true)]
        public string UserName { get; set; }
        [LogColumn("开始时间", IsLog = true)]
        public DateTime? StartTime { get; set; }
        [LogColumn("结束时间", IsLog = true)]
        public DateTime? EndTime { get; set; }
    }
    [AutoMapFrom(typeof(XZGLCarUserInfo), typeof(XZGLCarUserInfoInput))]
    public class XZGLCarUserInfoLogDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid? Id { get; set; }
        [LogColumn(@"车主", IsLog = true)]
        public string Name { get; set; }
        [LogColumn(@"车牌号", IsNameField = true)]
        public string CarNum { get; set; }
        [LogColumn(@"品牌型号", IsLog = true)]
        public string CarType { get; set; }
        [LogColumn(@"排量", IsLog = true)]
        public string Amount { get; set; }
        [LogColumn(@"座位数", IsLog = true)]
        public string SeatNum { get; set; }
        [LogColumn(@"驾驶证号", IsLog = true)]
        public string Number { get; set; }
        [LogColumn(@"驾驶证准驾车型", IsLog = true)]
        public string Type { get; set; }
        [LogColumn(@"行驶证车辆识别号", IsLog = true)]
        public string CarTypeNum { get; set; }
        [LogColumn(@"备注", IsLog = true)]
        public string Remark { get; set; }
    }
    public class XZGLCarInfoLogDto
    {
        [LogColumn("主键", IsLog = false)]
        public string Id { get; set; }
        /// <summary>
        /// 车辆编号
        /// </summary>
        [LogColumn(@"车辆", IsNameField = true)]
        public string CarNum{ get; set; }
        /// <summary>
        /// 司机
        /// </summary>
        [LogColumn(@"司机", IsLog = true)]
        public string UserName{ get; set; }
    }
    public class XZGLCarUserLogDto
    {

        [LogColumn("主键", IsLog = false)]
        public Guid? Id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        [LogColumn(@"平台名称", IsNameField = true)]
        public string Name { get; set; }

        /// <summary>
        /// 平台地址
        /// </summary>
        [LogColumn(@"平台地址", IsLog = true)]
        public string Address { get; set; }

        /// <summary>
        /// 平台联系方式
        /// </summary>
        [LogColumn(@"平台联系方式", IsLog = true)]
        public string Tel { get; set; }

        /// <summary>
        /// 租车费用
        /// </summary>
        [LogColumn(@"租车费用", IsLog = true)]
        public decimal CarMoney { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [LogColumn(@"订单编号", IsLog = true)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 车辆品牌型号
        /// </summary>
        [LogColumn(@"车辆品牌型号", IsLog = true)]
        public string CarType { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [LogColumn(@"车牌号", IsLog = true)]
        public string CarNum { get; set; }

    }
}