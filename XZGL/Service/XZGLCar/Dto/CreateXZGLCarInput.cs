using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace XZGL
{
    [AutoMapTo(typeof(XZGLCar))]
    public class CreateXZGLCarInput 
    {
        #region 表字段
        /// <summary>
        /// 车牌号
        /// </summary>
        [MaxLength(20,ErrorMessage = "车牌号长度必须小于20")]
        [Required(ErrorMessage = "必须填写车牌号")]
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        [MaxLength(20,ErrorMessage = "品牌型号长度必须小于20")]
        [Required(ErrorMessage = "必须填写品牌型号")]
        public string CarType { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        [MaxLength(2,ErrorMessage = "座位数长度必须小于2")]
        [Required(ErrorMessage = "必须填写座位数")]
        public string SeatNum { get; set; }

        /// <summary>
        /// 车身颜色
        /// </summary>
        [MaxLength(5,ErrorMessage = "车身颜色长度必须小于5")]
        [Required(ErrorMessage = "必须填写车身颜色")]
        public string CarColor { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [MaxLength(5,ErrorMessage = "排量长度必须小于5")]
        [Required(ErrorMessage = "必须填写排量")]
        public string Amount { get; set; }

        /// <summary>
        /// 变速箱
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public XZGLCarVariable Variable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 号牌号码
        /// </summary>
        [MaxLength(20,ErrorMessage = "号牌号码长度必须小于20")]
        [Required(ErrorMessage = "必须填写号牌号码")]
        public string Number { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [MaxLength(20,ErrorMessage = "车辆类型长度必须小于20")]
        [Required(ErrorMessage = "必须填写车辆类型")]
        public string Type { get; set; }

        /// <summary>
        /// 所有人
        /// </summary>
        [MaxLength(20,ErrorMessage = "所有人长度必须小于20")]
        [Required(ErrorMessage = "必须填写所有人")]
        public string UserName { get; set; }

        /// <summary>
        /// 使用性质
        /// </summary>
        [MaxLength(20,ErrorMessage = "使用性质长度必须小于20")]
        [Required(ErrorMessage = "必须填写使用性质")]
        public string UserType { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [MaxLength(200,ErrorMessage = "住址长度必须小于200")]
        [Required(ErrorMessage = "必须填写住址")]
        public string Address { get; set; }

        /// <summary>
        /// 行驶证品牌型号
        /// </summary>
        [MaxLength(20,ErrorMessage = "行驶证品牌型号长度必须小于20")]
        [Required(ErrorMessage = "必须填写行驶证品牌型号")]
        public string DrivingType { get; set; }

        /// <summary>
        /// 车辆识别代号
        /// </summary>
        [MaxLength(20,ErrorMessage = "车辆识别代号长度必须小于20")]
        [Required(ErrorMessage = "必须填写车辆识别代号")]
        public string DrivingNumber { get; set; }

        /// <summary>
        /// 发动机号码
        /// </summary>
        [MaxLength(20,ErrorMessage = "发动机号码长度必须小于20")]
        [Required(ErrorMessage = "必须填写发动机号码")]
        public string EngineNumber { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime CertificationTime { get; set; }

        /// <summary>
        /// 行驶证备注
        /// </summary>
        public string DrivingRemark { get; set; }


		
        #endregion
    }
}