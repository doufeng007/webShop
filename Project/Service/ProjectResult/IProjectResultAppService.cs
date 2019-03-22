using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectResultAppService : IApplicationService
    {

        /// <summary>
        /// 工程评审人员用的获取评审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectAuditResultOutput> GetAuditAsync(GetProjectResultForEditInput input);



        /// <summary>
        /// 汇总人员获取界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectHuiZongResultOutput> GetAuditForHuiZong(GetProjectResultForEditInput input);


        /// <summary>
        /// 获取初审结果汇总界面--联系人汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectFirstAuditCollectResultOutput> GetProjectFirstAuditCollect(GetProjectResultForEditInput input);


        /// <summary>
        /// 工程评审人 提交评审结果
        ///  Action 1 评审 2 汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateFinishResultAsync(CreateUpdateProjectAuditInput input);


        /// <summary>
        /// 获取 项目负责人 评审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectLeaderResultOutput> GetLeaderAuditAsync(GetProjectResultForEditInput input);

        /// <summary>
        /// 获取财务初审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<GetProjectAuditResultBaseOutput> GetCWFAuditAsync(GetProjectResultForEditInput input);


        /// <summary>
        /// 获取财务终审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectCWEResultOutput> GetCWEAuditAsync(GetProjectResultForEditInput input);


        /// <summary>
        /// 获取 复核 评审界面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectReviewResultOutput> GetReviewResultAsync(GetProjectResultForEditInput input);


        /// <summary>
        /// 项目负责人 复核人 提交评审结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Guid> CreateOrUpdateAsync(CreateOrUpdateProjectResultInput input);


        Task<ProjectAuditResultInfoOutput> GetAuditMemberResult(Guid projectId, int roleId, long? userId = null);


        Task<List<CreateOrUpdateFinishOutput>> GetFinishResult(Guid projectId, int auditRole, bool isForHuizongBySelf = false);














    }
}
