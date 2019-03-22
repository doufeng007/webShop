using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    public interface ICWGLAdvanceChargeAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CWGLAdvanceChargeListOutputDto>> GetList(GetCWGLAdvanceChargeListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CWGLAdvanceChargeOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个CWGLAdvanceCharge
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCWGLAdvanceChargeInput input);

		/// <summary>
        /// 修改一个CWGLAdvanceCharge
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCWGLAdvanceChargeInput input);

    }
}