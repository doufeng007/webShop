using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    public interface ICuringProcurementEditAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<CuringProcurementEditListOutputDto>> GetList(GetCuringProcurementEditListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<CuringProcurementEditOutputDto> Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 添加一个CuringProcurementEdit
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateCuringProcurementEditInput input);

        /// <summary>
        /// 修改一个CuringProcurementEdit
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateCuringProcurementEditInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        [RemoteService(IsEnabled = false)]
        void CompleteEdit(Guid instanceId);
    }
}