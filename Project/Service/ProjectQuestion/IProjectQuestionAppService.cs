using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectQuestionAppService: IApplicationService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Create(CreateProjectQuestionInput input);
        /// <summary>
        /// 项目经理获取列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<ProjectQuestionDto>> GetAllList(GetListInput input);
        /// <summary>
        /// 工程评审人获取自己的列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectQuestionDto>> GetMyList(GetListInput input);
        /// <summary>
        /// 项目经理回复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Answer(ProjectQuestionAnswerInput input);

    }
}
