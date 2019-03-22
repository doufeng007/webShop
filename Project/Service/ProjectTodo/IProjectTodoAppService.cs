using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectTodoAppService: IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectListStatus>> GetProjectListStatusAsync(SearchProjectListStatus input);


        Task<dynamic> GetProjectTodoCount(GetProjectTodoListInput input);
    }
}
