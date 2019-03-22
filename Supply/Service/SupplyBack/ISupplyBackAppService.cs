using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply.Service.SupplyBack
{
    /// <summary>
    /// 用品退还
    /// </summary>
    public interface ISupplyBackAppService: IApplicationService
    {
        /// <summary>
        /// 个人申请退还用品
        /// </summary>
        /// <returns></returns>
        InitWorkFlowOutput Create(CreateSupplyBackMainInput input);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        SupplyBackMainDto Get(GetWorkFlowTaskCommentInput input);
        void Update();
        /// <summary>
        /// 行政人员获取退换的商品列表
        /// </summary>
        Task<PagedResultDto<SupplyBackSubDto>>  GetSub(PageSubDto input);
        /// <summary>
        /// 行政人员确认商品退还
        /// </summary>
        Task Sure(SupplyBackInput input);
        /// <summary>
        /// 个人获取自己的退还记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SupplyBackMainDto>> GetAll(GetListInput input);
    }
}
