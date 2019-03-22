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
    public interface ICWGLPayMoneyAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CWGLPayMoneyListOutputDto>> GetList(GetCWGLPayMoneyListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CWGLPayMoneyOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个CWGLPayMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCWGLPayMoneyInput input);

		/// <summary>
        /// 修改一个CWGLPayMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCWGLPayMoneyInput input);

        void CreateCredential(Guid flowID, string InstanceID);
    }
}