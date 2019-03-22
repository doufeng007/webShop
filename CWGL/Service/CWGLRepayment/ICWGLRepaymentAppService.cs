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
    public interface ICWGLRepaymentAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CWGLRepaymentListOutputDto>> GetList(GetCWGLRepaymentListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CWGLRepaymentOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个CWGLRepayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCWGLRepaymentInput input);

		/// <summary>
        /// 修改一个CWGLRepayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCWGLRepaymentInput input);

    }
}