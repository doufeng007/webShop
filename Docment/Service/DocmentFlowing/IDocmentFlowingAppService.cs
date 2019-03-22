using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Docment
{
    public  interface IDocmentFlowingAppService: IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        List<DocmentFlowingDto> GetAll(Guid docmentId);
        /// <summary>
        /// 创建外部流传记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOutFlowing(DocmentFlowingInput input);
        /// <summary>
        /// 档案统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentStaticDto>> GetStatic(DocmentStaticSearshDto input);
        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentListDto>> GetDocmentList(SearchInput input);
    }
}
