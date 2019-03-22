using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using HR.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public interface IEmployeePlanAppService: IApplicationService
    {
        /// <summary>
        /// 面试人员创建预约登记
        /// </summary>
        Task<InitWorkFlowOutput> Create(CreatePlanInput input);
        /// <summary>
        /// 面试人员修改预约登记
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(EmployeePlanEditInput input);
        /// <summary>
        /// 获取预约面试列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeePlanListDto>> GetList(EmployeePlanSearchInput input);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task< EmployeePlanDto> Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 人力资源安排面试时间和面试官
        /// </summary>
        /// <param name="input"></param>
        Task UpdatePlan(EmployeePlanInput input);
        /// <summary>
        /// 登记员、汇总员登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateComment(EmployeePlanCommentInput input);
        /// <summary>
        /// 分管领导登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateVerify(EmployeePlanResultInput input);
        /// <summary>
        /// 领导登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAdminVerify(EmployeePlanAdminResultInput input);

        Task UpdateJoin(EmployeeJoinInput input);

        /// <summary>
        /// 面试后更新人才库状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="statue"></param>
        void ChangeResumeStatus(ChangeStatusInput input);
    }
}
