using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    [AutoMap(typeof(Employee))]
    public class EmployeeListDto
    {
        public Guid Id { get; set; }
        /// 工号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否离职
        /// </summary>
        public bool IsResign { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string WXchat { get; set; }

        /// <summary>
        /// 支付宝
        /// </summary>
        public string Alipay { get; set; }
        /// <summary>
        /// 工资卡号
        /// </summary>
        public string BankNo { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EnterTime { get; set; }
        public long? UserId { get; set; }
        /// <summary>
        /// 员工部门
        /// </summary>
        public List<SimpleOrganizationDto> Organization { get; set; }
        /// <summary>
        /// 员工岗位
        /// </summary>
        public List<UserPostDto> Posts { get; set; }
    }
}
