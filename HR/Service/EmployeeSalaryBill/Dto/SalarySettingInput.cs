using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Service
{
    /// <summary>
    /// 设置工资条编辑时间和展示天数
    /// </summary>
    public  class SalarySettingInput
    {
        /// <summary>
        /// 每月的几号
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 展示天数
        /// </summary>
        public int Length { get; set; }
       
    }

    public class SalarySettingDto: SalarySettingInput
    {
        /// <summary>
        /// 定时器编号
        /// </summary>
        public Guid? HangFilreId { get; set; }
    }
}
