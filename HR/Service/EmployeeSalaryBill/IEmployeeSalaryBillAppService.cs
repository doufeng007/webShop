using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR.Service
{
    /// <summary>
    /// 员工工资条
    /// </summary>
    public  interface IEmployeeSalaryBillAppService: IApplicationService
    {
        /// <summary>
        /// 人事填报工资条
        /// </summary>
        void CreateOrUpdate(List<EmployeeSalaryBillInput> input );

        
        /// <summary>
        /// 人事获取本年以前月份的工资条
        /// </summary>
         Task<List<EmployeeSalaryBillDto>> GetList(Guid? input);
        /// <summary>
        /// 个人获取本年月份工资明细
        /// </summary>
        Task<List<EmployeeSalaryBillForMe>> GetSalaryForMe();

        /// <summary>
        /// 个人获取本年度月份奖金明细
        /// </summary>
        Task<List<EmployeeSalaryBillBonusForMe>> GetBonusForMe();

        Task<EmployeeSalaryDto> GetAllEmployee(EmployeeSearchInput input);
        /// <summary>
        /// 设置工资发放配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetSetting(SalarySettingInput input);
        /// <summary>
        /// 获取工资发放配置
        /// </summary>
        /// <returns></returns>
        Task< SalarySettingInput> GetSetting();
    }
}
