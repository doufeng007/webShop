using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// 员工基本信息
    /// </summary>
    [AutoMap(typeof(Employee))]
    public class EmployeeDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        
        public string Name { get; set; }
        /// <summary>
        /// 性别0：男  1：女
        /// </summary>
        
        public int Sex { get; set; }
        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// 生日
        /// </summary>

        public DateTime Birthday { get; set; }
        /// <summary>
        /// 农历生日
        /// </summary>
        public string Birthday2 { get; set; }
        /// <summary>
        /// 电话
        /// </summary>

        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        
        public string Email { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string WXchat { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 最高学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 兴趣爱好
        /// </summary>
        public string Enjoy { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>

        public string Alipay { get; set; }
        /// <summary>
        /// 工资卡号
        /// </summary>

        public string BankNo { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行类别
        /// </summary>
        public Guid? BankType { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string BankAddress { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EnterTime { get; set; }
        /// <summary>
        /// 教育经历
        /// </summary>
        public List<EducationExperienceDto> EducationExperience { get; set; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public List<WorkExperienceDto> WorkExperience { get; set; }
        /// <summary>
        /// 家庭情况
        /// </summary>
        public List<EmployeeFamilyDto> EmployeeFamily { get; set; }
        /// <summary>
        /// 技能情况
        /// </summary>
        public List<EmployeeSkillDto> EmployeeSkill { get; set; }
        /// <summary>
        /// 员工部门
        /// </summary>
        public List<SimpleOrganizationDto> Organization { get; set; }
        /// <summary>
        /// 员工岗位
        /// </summary>
        public List<UserPostDto> Posts { get; set; }
        public long? UserId { get; set; }
    }
}
