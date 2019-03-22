using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public interface IEmployeeResignAppService : IApplicationService
    {
        /// <summary>
        /// 创建离职申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateEmployeeResignInput input);
        /// <summary>
        /// 更新离职申请
        /// </summary>
        /// <param name="input"></param>
        Task Update(UpdateEmployeeResignInput input);
        /// <summary>
        /// 获取申请详情
        /// </summary>
        /// <returns></returns>
        Task<EmployeeResignDto> Get(GetWorkFlowTaskCommentInput input);
        /// <summary>
        /// 获取草稿状态的离职申请
        /// </summary>
        /// <returns></returns>
        Task<EmployeeResignDto> GetDraft();
        /// <summary>
        /// 获取自己的离职申请列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeResignListDto>> GetAll(WorkFlowPagedAndSortedInputDto input);
        bool EmployeeResignSuccess(Guid guid);

        bool EmployeeResignIsLeader(Guid guid);
    }
}
