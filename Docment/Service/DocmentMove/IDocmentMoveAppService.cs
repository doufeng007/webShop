using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using Docment.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Docment
{
    public  interface IDocmentMoveAppService : IApplicationService
    {
        /// <summary>
        /// 创建归档申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task< InitWorkFlowOutput> Create(CreateDocmentMoveInput input);
        /// <summary>
        /// 更新归档申请
        /// </summary>
        /// <param name="input"></param>
        Task Update(UpdateeDocmentMoveInput input);
        /// <summary>
        /// 获取归档申请详情
        /// </summary>
        /// <returns></returns>
        Task<DocmentMoveDto> Get(GetWorkFlowTaskCommentInput input);
        ///// <summary>
        ///// 获取档案列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //Task<PagedResultDto<DocmentMoveListDto>> GetAll(DocmentSearchInput input);
        /// <summary>
        /// 档案移交申请记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentMoveListDto>> GetAll(DocmentMoveSearchDto input);

        Task StopMove(Guid id, Guid flowId, string reason);
       
    }
}
