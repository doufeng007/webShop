using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeDocmentAppService: IApplicationService
    {
        /// <summary>
        /// 获取员工档案信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeDocmentListDto>> GetList(EmployeeSearchInput input);
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(EmployeeDocmentFileInput input);
        /// <summary>
        /// 获取指定员工的档案
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        EmployeeDocmentListDto Get(Guid employeeId);
    }
}
