using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XZGL
{
    public class UpdateXZGLCarBorrowInput : CreateXZGLCarBorrowInput
    {
        public Guid FlowId { get; set; }
        public Guid InStanceId { get; set; }
        public bool IsUpdateForChange { get; set; }
        public List<XZGLCarUserInput> CarUser { get; set; } = new List<XZGLCarUserInput>();
        public List<XZGLCarInfoInput> CarInfo { get; set; } = new List<XZGLCarInfoInput>();
        public XZGLCarUserInfoInput CarUserInfo { get; set; }
        public List<Guid> RelationWorkOut { get; set; } = new List<Guid>();
        public List<XZGLWorkoutListByCarDto> OldWorkouts { get; set; } = new List<XZGLWorkoutListByCarDto>();
        public List<XZGLWorkoutListByCarDto> NewWorkouts { get; set; } = new List<XZGLWorkoutListByCarDto>();

    }
    public class UpdateXZGLCarInfoInput {
        public Guid CarBorrowId { get; set; }
        public string Remark { get; set; }
    }
    public class XZGLCarUserInfoInput
    {
        /// <summary>
        /// 车主
        /// </summary>
        [DisplayName(@"车主")]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        [MaxLength(20)]
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        [DisplayName(@"品牌型号")]
        [MaxLength(20)]
        public string CarType { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [DisplayName(@"排量")]
        [MaxLength(20)]
        public string Amount { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        [DisplayName(@"座位数")]
        [MaxLength(20)]
        public string SeatNum { get; set; }

        /// <summary>
        /// 驾驶证号
        /// </summary>
        [DisplayName(@"驾驶证号")]
        [MaxLength(20)]
        public string Number { get; set; }

        /// <summary>
        /// 驾驶证准驾车型
        /// </summary>
        [DisplayName(@"驾驶证准驾车型")]
        [MaxLength(20)]
        public string Type { get; set; }

        /// <summary>
        /// 行驶证车辆识别号
        /// </summary>
        [DisplayName(@"行驶证车辆识别号")]
        [MaxLength(20)]
        public string CarTypeNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }
    }
    public class XZGLCarInfoInput
    {
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
    }
    public class XZGLCarUserInput
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        [DisplayName(@"平台名称")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 平台地址
        /// </summary>
        [DisplayName(@"平台地址")]
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 平台联系方式
        /// </summary>
        [DisplayName(@"平台联系方式")]
        [MaxLength(50)]
        [Required]
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
        [MaxLength(50)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 车辆品牌型号
        /// </summary>
        [DisplayName(@"车辆品牌型号")]
        [MaxLength(50)]
        [Required]
        public string CarType { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        [MaxLength(50)]
        [Required]
        public string CarNum { get; set; }

    }
    public class RelationInput
    {
        public Guid Id { get; set; }
        public List<Guid> CarBorrowIds { get; set; }
        public CarRelationType TypeId { get; set; }
    }
}