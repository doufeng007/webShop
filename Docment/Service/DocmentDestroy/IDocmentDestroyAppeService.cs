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
    /// <summary>
    /// 档案销毁
    /// </summary>
    public interface IDocmentDestroyAppeService:IApplicationService
    {
        /// <summary>
        /// 创建销毁申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateDocmentDestroyInput input);
        /// <summary>
        /// 更新销毁申请
        /// </summary>
        /// <param name="input"></param>
        Task Update(UpdateeDocmentDestroyInput input);
        /// <summary>
        /// 获取销毁申请详情
        /// </summary>
        /// <returns></returns>
        Task<DocmentDestroyDto> Get(GetWorkFlowTaskCommentInput input);
        /// <summary>
        /// 档案销毁申请记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentDestroyListDto>> GetAll(DocmentDestroySearchDto input);

        Task StopDestroy(Guid id, Guid flowId, string reason);
        
    }
}
