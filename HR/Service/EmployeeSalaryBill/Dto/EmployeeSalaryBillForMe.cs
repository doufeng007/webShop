using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 个人查看工资条
    /// </summary>
    [AutoMap(typeof(EmployeeSalaryBill))]
    public  class EmployeeSalaryBillForMe
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// 工资条年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 工资条月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 实发工资
        /// </summary>
        public string Salary { get; set; }
    }
}
