using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    public interface IQuickLinkBaseAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<QuickLinkBaseListOutputDto>> GetList(GetQuickLinkBaseListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<QuickLinkBaseOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个QuickLinkBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateQuickLinkBaseInput input);

		/// <summary>
        /// 修改一个QuickLinkBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateQuickLinkBaseInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}