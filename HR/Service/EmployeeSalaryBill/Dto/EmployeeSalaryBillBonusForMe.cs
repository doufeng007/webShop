using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 个人查看奖金福利
    /// </summary>
    [AutoMap(typeof(EmployeeSalaryBill))]
    public class EmployeeSalaryBillBonusForMe
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工资条年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 工资条月份
        /// </summary>
        public int Month { get; set; }
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
        /// 实发工资
        /// </summary>
        public string Salary { get; set; }
    }
}
