using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore;

namespace GWGL
{
    public interface IEmployees_SignAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<Employees_SignListOutputDto>> GetList(GetEmployees_SignListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<Employees_SignOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个Employees_Sign
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateEmployees_SignInput input);

		/// <summary>
        /// 修改一个Employees_Sign
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateEmployees_SignInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);


        Task<List<ChangeLog>> GetChangeLogList(EntityDto<Guid> input);
    }
}