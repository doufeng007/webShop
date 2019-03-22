using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectMemberAppService : IApplicationService
    {
        /// <summary>
        /// 评审组 评审人员分派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProjectAuditMembers(ManagerProjectAuditMembersInput input);


        /// <summary>
        /// 项目负责人分派工程评审
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateFinishAsync(ManagerProjectAuditMembersInput input);
        

        /// <summary>
        /// 指定复核人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task UpdateProjectSpecifyReviewAsync(CreateProjectSpecifyReviewInput input);


        /// <summary>
        /// 获取项目分派情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectMemberForEditOutput> GetAsync(EntityDto<Guid> input);

        /// <summary>
        /// 获取项目的评审人员  主要是member表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<List<CreateOrUpdateAuditMemberOutput>> GetProjectAuditMembers(GetProjectAuditMembersInput input);


        List<CreateOrUpdateAuditMemberOutput> GetProjectAuditMember(GetProjectAuditMembersInput input);


        /// <summary>
        /// 更新控制表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProjectControlAuditResult(UpdateProjectControlAuditResultInput input);

        /// <summary>
        /// 项目评审人开始评审
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        Task BeginProjectAudit(Guid projectId, Guid taskId);



        /// <summary>
        /// 开始执行按钮对应api  暂时实现
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="instanceId"></param>
        /// <param name="type">1 开始项目评审 2开始任务 3开始维修固定资产</param>
        /// <returns></returns>
        Task BeginButtonApi(BeginFlowerDto input);

    }




}
