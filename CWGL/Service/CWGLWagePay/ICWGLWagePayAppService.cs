using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    public interface ICWGLWagePayAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<CWGLWagePayListOutputDto>> GetList(GetCWGLWagePayListInput input);


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<CWGLWagePayOutputDto> Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 添加一个CWGLWagePay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateCWGLWagePayInput input);


        /// <summary>
        /// Hangfire自动创建
        /// </summary>
        void AutoCreate();

        /// <summary>
        /// 修改一个CWGLWagePay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateCWGLWagePayInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);
    }
}