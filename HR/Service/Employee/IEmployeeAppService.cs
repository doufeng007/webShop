using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR.Service
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public interface IEmployeeAppService: IApplicationService
    {
        /// <summary>
        /// 员工个人信息初始化
        /// </summary>
        Task CreateOrUpdate(EmployeeInput input);

        /// <summary>
        /// 员工个人信息
        /// </summary>
       Task<EmployeeDto> Get(Nullable<Guid> input);
        /// <summary>
        /// 员工个人信息
        /// </summary>
        Task<EmployeeDto> GetByUserId(Nullable<long> input);

        /// <summary>
        /// 人事部获取员工信息列表
        /// </summary>
        Task<PagedResultDto<EmployeeListDto>> GetList(EmployeeSearchInput input);
        string SolarToChineseLunisolarDate(DateTime solarDateTime);
    }
}
