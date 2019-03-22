using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public interface IProjectTodoNewAppService : IApplicationService
    {
        /// <summary>
        /// 获取团队内项目的待办、在办、已办统计
        /// </summary>
        /// <returns></returns>
        ProjectTodoNewDto GetProjectStatic();
        /// <summary>
        /// 获取流程待办和我的待办
        /// </summary>
        /// <param name="todoType">0：待办 1：在办</param>
        /// <returns></returns>
        PagedResultDto<ProjectTodoNewListDto> GetWorkTaskList(SearchTodoInput input);

        /// <summary>
        /// 统计我的待办
        /// </summary>
        /// <returns></returns>
        ProjectTodoNewDto GetProjectStaticForMy();
        /// <summary>
        /// 获取在办待办已办项目统计表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagedResultDto<ProjectListStatus> GetProjectListStatus(SearchProjectListStatus input);
        /// <summary>
        /// 获取日常待办事项统计表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagedResultDto<OATodoList> GetOATodoList(SearchProjectListStatus input);
    }
}
