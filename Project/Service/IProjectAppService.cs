using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectAppService : IApplicationService
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>


        Task<List<CreateOrUpdateProjectFileInput>> GetPreProjectFiles(int appraisalTypeId);


        Task<InitWorkFlowOutput> CreateAsync(CreateOrUpdateProJectBudgetManagerInput input);

        Task<GetProjectBudgetForEditOutput> GetProjectBudgetForEdit(GetProjectForEditInput input);


        Task<GetProjectBudgetForEditOutput> GetSingleProject(GetSingleProjectInput input);

        Task<PagedResultDto<GetSingleProjectInfoListOutput>> GetSingleProjectInfoList(GetSingleProjectInfoListInput input);
        Task<GetProjectBudgetForEditOutput> GetProjectSingleAllInfo(GetSingleProjectInput input);

        Task UpdateAsync(CreateOrUpdateProJectBudgetManagerInput input);


        Task UpdateForChangeAsync(ProJectBudgetUpdateChangeInput input);


        Task<GetProjectAuditSmmaryOutput> GetReturnAuditSmmary(NullableIdDto<Guid> input);

        //Task DeleteAsync(EntityDto<Guid> input);


        Task<PagedResultDto<ProjectListGroupWithCodeDto>> GetProjectList(GetProjectListInput input);


        Task<List<ProjectSingleListDto>> GetSingleProjectByProjectCode(string projectCode);


        Task<ProjectTodoStaticDto> GetTodoStatic();


        string GetPy(string name);

        /// <summary>
        /// 退回审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ReturnAuditProject(ReturnAuditProjectInput input);


        bool GetIsProjectLeader(Guid projectId);


        bool GetIsCreateByProjecetLeader(Guid taskId);



        Task<List<CreateOrUpdateProjectFileInput>> GetProjectFiles(GetProjectFileInput input);

        /// <summary>
        /// 强行提交验证缺少字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ValidateForPorjectResultOutput> ValilationProjectModel(
           Guid projectId);

        /// <summary>
        /// 关注项目  取消关注
        /// </summary>
        /// <param name="input"></param>
        void FollowProject(Guid input);
        /// <summary>
        /// 项目负责人打开待办时  设置项目准备完成。
        /// </summary>
        /// <param name="input"></param>
        void SetReadyEndTime(Guid input);

        /// <summary>
        /// 部门领导审核后 设置项目准备开始
        /// </summary>
        /// <param name="input"></param>
        void SetReadyStartTime(Guid input);


        [RemoteService(IsEnabled =false)]
        Dictionary<Guid, string> SingleProjectFlowActive(Guid projectId, long userId);
    }




}
