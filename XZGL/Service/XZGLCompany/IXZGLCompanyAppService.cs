using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace XZGL
{
    public interface IXZGLCompanyAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<XZGLCompanyListOutputDto>> GetList(GetXZGLCompanyListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<XZGLCompanyOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个XZGLCompany
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateXZGLCompanyInput input);

		/// <summary>
        /// 修改一个XZGLCompany
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateXZGLCompanyInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}