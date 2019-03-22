using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeOtherInfoAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<EmployeeOtherInfoListOutputDto>> GetList(GetEmployeeOtherInfoListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<EmployeeOtherInfoOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个EmployeeOtherInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateEmployeeOtherInfoInput input);

		/// <summary>
        /// 修改一个EmployeeOtherInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(EmployeeOtherInfo input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}