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
    [AutoMapFrom(typeof(XZGLCar))]
    public class XZGLCarListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        public string SeatNum { get; set; }

        /// <summary>
        /// 车身颜色
        /// </summary>
        public string CarColor { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 变速箱
        /// </summary>
        public XZGLCarVariable Variable { get; set; }
        public XZGLCarStatus Status { get; set; }
        public string StatusName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 号牌号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 所有人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用性质
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 行驶证品牌型号
        /// </summary>
        public string DrivingType { get; set; }

        /// <summary>
        /// 车辆识别代号
        /// </summary>
        public string DrivingNumber { get; set; }

        /// <summary>
        /// 发动机号码
        /// </summary>
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

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public bool IsEnable { get; set; }


    }
}
