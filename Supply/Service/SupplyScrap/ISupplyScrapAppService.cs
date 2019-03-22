using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using Supply.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public interface ISupplyScrapAppService : IApplicationService
    {
        /// <summary>
        /// 创建报废申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<InitWorkFlowOutput> Create(CreateSupplyScrapMainInput input);


        InitWorkFlowOutput CreateV2(CreateSupplyScrapMainInput input);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetSupplyScrapSubDto Get(GetWorkFlowTaskCommentInput input);
        //void Update();
        /// <summary>
        /// 报废管理
        /// </summary>
        Task<PagedResultDto<SupplyScrapSubDto>> GetList(SupplyScrapListInput input);
        /// <summary>
        /// 行政人员确认商品报废
        /// </summary>
        Task Sure(SupplyScrapInput input);
        /// <summary>
        /// 获取我的报废记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SupplyScrapMainDto>> GetAll(GetListInput input);


        Task SubmitSupplyScrap(List<SubmitSupplyScrapInput> input);


        [RemoteService(IsEnabled =false)]
        string CreateSubFlowSupplyScrapInstance(string instanceId);

        [RemoteService(IsEnabled = false)]
        void AfterScrapAction(string instanceId, Guid flowId);


        GetSupplyScrapMainDto GetMain(GetWorkFlowTaskCommentInput input);



        Task Update(SupplyScrapUpdateInput input);
    }

}
