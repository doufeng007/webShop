using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// 工资条信息
    /// </summary>
    [AutoMap(typeof(EmployeeSalaryBill))]
    public class EmployeeSalaryBillInput
    {
        public Guid? Id { get; set; }
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// 工资条年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 工资条月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal BaseSalary { get; set; }
        /// <summary>
        /// 补助工资
        /// </summary>
        public decimal WorkSalary { get; set; }
        /// <summary>
        /// 月奖金
        /// </summary>
        public decimal MonthBonus { get; set; }
        /// <summary>
        /// 季度奖金
        /// </summary>
        public decimal QuarterBonus { get; set; }
        /// <summary>
        /// 年奖金
        /// </summary>
        public decimal YearBonus { get; set; }
        /// <summary>
        /// 绩效奖金
        /// </summary>
        public decimal WorkBonus { get; set; }
        /// <summary>
        /// 应发工资
        /// </summary>
        public decimal PreSalary { get; set; }
        /// <summary>
        /// 养老保险
        /// </summary>
        public decimal EndowmentInsurance { get; set; }
        /// <summary>
        /// 医疗保险
        /// </summary>
        public decimal MedicalInsurance { get; set; }
        /// <summary>
        /// 生育保险
        /// </summary>
        public decimal MaternityInsurance { get; set; }
        /// <summary>
        /// 工伤保险
        /// </summary>
        public decimal InjuryInsurance { get; set; }
        /// <summary>
        /// 失业保险
        /// </summary>
        public decimal UnworkInsurance { get; set; }
        /// <summary>
        /// 公积金
        /// </summary>
        public decimal AccumulationFund { get; set; }
        /// <summary>
        /// 所得税
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// 扣款
        /// </summary>
        public decimal Deduction { get; set; }
        /// <summary>
        /// 扣款原因
        /// </summary>
        public string DeductionReason { get; set; }
        /// <summary>
        /// 实发工资
        /// </summary>
        public decimal Salary { get; set; }
    }

    /// <summary>
    /// 人事查看工资条
    /// </summary>
    [AutoMap(typeof(EmployeeSalaryBill))]
    public class EmployeeSalaryBillDto:EmployeeSalaryBillInput {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
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

    public class EmployeeSalaryDto {
        /// <summary>
        /// 工资详情
        /// </summary>
        public PagedResultDto<EmployeeSalaryBillDto> Items { get; set; }
        /// <summary>
        /// 工资年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 工资月份
        /// </summary>
        public int Month { get; set; }
    }
}
