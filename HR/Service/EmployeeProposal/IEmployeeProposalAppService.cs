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
    public interface IEmployeeProposalAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<EmployeeProposalListOutputDto>> GetList(GetEmployeeProposalListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<EmployeeProposalOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个EmployeeProposal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateEmployeeProposalInput input);

        /// <summary>
        /// 修改一个EmployeeProposal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateEmployeeProposalInput input);
        [RemoteService(false)]
        void InsertIssue(Guid id);
    }
}