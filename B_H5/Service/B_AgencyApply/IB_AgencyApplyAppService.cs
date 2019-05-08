using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    public interface IB_AgencyApplyAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_AgencyApplyListOutputDto>> GetList(GetB_AgencyApplyListInput input);



        Task<B_AgencyApplyCount> GetCount();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_AgencyApplyOutputDto> Get(EntityDto<Guid> input);

        /// <summary>
        /// 添加一个B_AgencyApply
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<Guid> Create(CreateB_AgencyApplyInput input);

		/// <summary>
        /// 修改一个B_AgencyApply
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Audit(AuditB_AgencyApplyInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}