using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using HR;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// 员工基本信息
    /// </summary>
    [AutoMap(typeof(Employee))]
    public class EmployeeInput
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
        [Required(ErrorMessage = "性别必须填写")]
        public int Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Required(ErrorMessage = "生日必须填写")]
        public DateTime Birthday { get; set; }
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
        /// 通讯地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 最高学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EnterTime { get; set; }

        public string IdCard { get; set; }
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

    }
    /// <summary>
    /// 教育经历
    /// </summary>
    [AutoMap(typeof(EducationExperience))]
    public class EducationExperienceDto {
        
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
    }
    /// <summary>
    /// 工作经历
    /// </summary>
    [AutoMap(typeof(WorkExperience))]
    public class WorkExperienceDto {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName { get; set; }
    }
    /// <summary>
    /// 家庭情况
    /// </summary>
    [AutoMap(typeof(EmployeeFamily))]
    public class EmployeeFamilyDto {
        public Guid? Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
    }
    /// <summary>
    /// 技能评价
    /// </summary>
    [AutoMap(typeof(EmployeeSkill))]
    public class EmployeeSkillDto {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 熟练程度1-10分
        /// </summary>
        public int Level { get; set; }
    }
}
