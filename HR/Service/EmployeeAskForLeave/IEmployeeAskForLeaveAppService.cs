using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace HR
{
    public interface IEmployeeAskForLeaveAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeAskForLeaveListOutputDto>> GetEmployeeAskForLeaveList(GetEmployeeAskForLeaveListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<EmployeeAskForLeaveOutputDto> GetEmployeeAskForLeave(GetWorkFlowTaskCommentInput input);
        Task<int> GetEmployeeAskForLeaveMonthCount();
        /// <summary>
        /// 添加/修改一个EmployeeAskForLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateEmployeeAskForLeaveIputDto input);




        Task Update(UpdateEmployeeAskForLeaveIputDto input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task DeleteEmployeeAskForLeave(EntityDto<Guid> input);


        /// <summary>
        /// 获取变更记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ChangeLog>> GetChangeLog(EntityDto<Guid> input, Guid flowId);
        void AskForLeaveRelationUserId(Guid instanceID);
    }
}